FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Security.Service.API/Security.Service.API.csproj", "Security.Service.API/"]
COPY ["Security.Domain.Repository/Security.Domain.Repository.csproj", "Security.Domain.Repository/"]
COPY ["Domain.Models/Security.Domain.Models.csproj", "Domain.Models/"]
COPY ["Security.Services/Security.Services.csproj", "Security.Services/"]
COPY ["Security.ViewModel/Security.Service.ViewModel.csproj", "Security.ViewModel/"]
COPY ["Security.Infrustructure/Security.Infrustructure.csproj", "Security.Infrustructure/"]
COPY ["Security.View.Mapper/Security.View.ObjectMapper.csproj", "Security.View.Mapper/"]
RUN dotnet restore "Security.Service.API/Security.Service.API.csproj"
COPY . .
WORKDIR "/src/Security.Service.API"
RUN dotnet build "Security.Service.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Security.Service.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Security.Service.API.dll"]