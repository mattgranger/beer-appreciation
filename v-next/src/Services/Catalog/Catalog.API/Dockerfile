FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Services/Catalog/Catalog.API/Catalog.API.csproj src/Services/Catalog/Catalog.API/
RUN dotnet restore src/Services/Catalog/Catalog.API/Catalog.API.csproj
COPY . .
WORKDIR /src/src/Services/Catalog/Catalog.API
RUN dotnet build Catalog.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Catalog.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
