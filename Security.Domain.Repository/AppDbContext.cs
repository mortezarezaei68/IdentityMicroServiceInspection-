using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Security.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRegister>(b =>
            {
                b.Ignore(a => a.ConfirmPassword);
                b.Ignore(a => a.Password);
            });
        }
        DbSet<UserRegister> userRegisters { get; set; }
        DbSet<UserRole> userRoles { get; set; }
    }
}
