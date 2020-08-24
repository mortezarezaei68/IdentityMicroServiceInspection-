using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Security.Domain.Models;
using Security.Infrustructure;
using Security.Services;
using Security.Services.IdentityEmailCustomConfiguration;
using Security.View.ObjectMapper;
using Swashbuckle.AspNetCore.Swagger;

namespace Security.Service.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<UserRegister, UserRole>(
                //config =>
                //{
                //    config.SignIn.RequireConfirmedEmail = true;
                //    config.User.RequireUniqueEmail = true;
                //    config.Tokens.ProviderMap.Add("MySuperSecureKey", new TokenProviderDescriptor(
                //typeof(CustomEmailConfirmationTokenProvider<IdentityUser>)));
                //config.Tokens.EmailConfirmationTokenProvider = "MySuperSecureKey";
                //}
    )
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders().AddRoleManager<RoleManager<UserRole>>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://localhost",
                    ValidIssuer = "http://localhost",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"))
                };
            });
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<CustomEmailConfirmationTokenProvider<IdentityUser>>();
            services.AddTransient<IPasswordGeneratorFactory,PasswordGeneratorFactory>();
            services.AddTransient<IRequestResponseFactory, RequestResponseFactory>();
            services.AddTransient<ICustomAuthorizeService, CustomAuthorizeService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRegisterService, RegisterService>();
            services.AddTransient<ICompanyRegisterService, CompanyRegisterService>();
            services.AddAuthorization();
            services.AddAutoMapper(typeof(ViewMapper));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Values Api", Version = "v1" });
            });
            services.AddTransient<IUserRegisterService, UserRegisterService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<EmailSettingDomain>(Configuration.GetSection("EmailSettings"));
            services.Configure<SMSSetting>(Configuration.GetSection("MessageSetting"));
            //services.AddSingleton<ISMSSender, SMSSender>();
            //services.AddSingleton<IMessageSender, EmailSender>();
            //services.AddSingleton<IMessageSender, SMSSender>();
            services.AddTransient<IMessageSenderFactory, MessageSenderFactory>();
            services.Configure<DataProtectionTokenProviderOptions>(o =>
   o.TokenLifespan = TimeSpan.FromHours(3));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
               // context.Database.Migrate();
                context.Database.EnsureCreated();
            }
            if (env.IsDevelopment())
            {

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";

                        var exceptionHandlerPathFeature =
                                                    context.Features.Get<IExceptionHandlerPathFeature>();
                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync($"ERROR!<br><br>\r\n<p>InnerException:{exceptionHandlerPathFeature.Error.InnerException}\r\n" +
                            $"</p><p>Message:{exceptionHandlerPathFeature.Error.Message}</p>\r\n" +
                            $"<p>Trace:\r\n<br><br>{exceptionHandlerPathFeature.Error.StackTrace}</p>");
                        


                        // Use exceptionHandlerPathFeature to process the exception (for example, 
                        // logging), but do NOT expose sensitive error information directly to 
                        // the client.

                        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                        {
                            await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
                        }

                        //await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
                        //await context.Response.WriteAsync("</body></html>\r\n");
                        //await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                    });
                });
                app.UseHsts();
            }
            else
            {

            }
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Values Api V1");
            });
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc();
        }
    }
}
