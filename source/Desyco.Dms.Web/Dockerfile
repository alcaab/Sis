FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Netto.Kassentableau.Web/Netto.Kassentableau.Web.csproj", "Netto.Kassentableau.Web/"]
RUN dotnet restore "Netto.Kassentableau.Web/Netto.Kassentableau.Web.csproj"
COPY . .
WORKDIR "/src/WebApplication1"
RUN dotnet build "Netto.Kassentableau.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Netto.Kassentableau.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Netto.Kassentableau.Web.dll"]
