#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DatingApp.WebAPI/DatingApp.WebAPI.csproj", "DatingApp.WebAPI/"]
COPY ["DatingApp.Infrastructure/DatingApp.Infrastructure.csproj", "DatingApp.Infrastructure/"]
COPY ["DatingApp.Core/DatingApp.Core.csproj", "DatingApp.Core/"]
COPY ["DatingApp.Business/DatingApp.Business.csproj", "DatingApp.Business/"]
RUN dotnet restore "DatingApp.WebAPI/DatingApp.WebAPI.csproj"
COPY . .
WORKDIR "/src/DatingApp.WebAPI"
RUN dotnet build "DatingApp.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DatingApp.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DatingApp.WebAPI.dll"]