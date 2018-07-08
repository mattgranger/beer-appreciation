FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Services/Events/EventManagement.API/EventManagement.API.csproj src/Services/Events/EventManagement.API/
RUN dotnet restore src/Services/Events/EventManagement.API/EventManagement.API.csproj
COPY . .
WORKDIR /src/src/Services/Events/EventManagement.API
RUN dotnet build EventManagement.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish EventManagement.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EventManagement.API.dll"]
