# build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY . .
RUN dotnet restore PhotoServiceAPI.sln
RUN dotnet build -c Release PhotoServiceAPI.sln
RUN dotnet publish -c Release WebApp/WebApp.csproj -o build

# runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "PhotoServiceAPI.dll"]