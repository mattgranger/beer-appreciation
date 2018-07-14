FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Beverage.API/Beverage.API.csproj Beverage.API/
RUN dotnet restore Beverage.API/Beverage.API.csproj
COPY . .
WORKDIR /src/Beverage.API
RUN dotnet build Beverage.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Beverage.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Beverage.API.dll"]
