#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["Security.Service.API/Security.Service.API.csproj", "Security.Service.API/"]
COPY ["Security.Services/Security.Services.csproj", "Security.Services/"]
COPY ["Security.ViewModel/Security.Service.ViewModel.csproj", "Security.ViewModel/"]
COPY ["Domain.Models/Security.Domain.Models.csproj", "Domain.Models/"]
RUN dotnet restore "Security.Service.API/Security.Service.API.csproj"
COPY . .
WORKDIR "/src/Security.Service.API"
RUN dotnet build "Security.Service.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Security.Service.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Security.Service.API.dll"]