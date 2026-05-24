FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY AgriEnergyConnect/AgriEnergyConnect.csproj ./
RUN dotnet restore
COPY AgriEnergyConnect/ ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "AgriEnergyConnect.dll"]
