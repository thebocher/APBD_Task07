FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/APBD_Task07.App/APBD_Task07.App.csproj", "src/APBD_Task07.App/"]
COPY ["src/APBD_Task07.Model/APBD_Task07.Model.csproj", "src/APBD_Task07.Model/"]
COPY ["src/APBD_Task07.Logic/APBD_Task07.Logic.csproj", "src/APBD_Task07.Logic/"]
RUN dotnet restore "src/APBD_Task07.App/APBD_Task07.App.csproj"
COPY . .
WORKDIR "/src/src/APBD_Task07.App"
RUN dotnet build "APBD_Task07.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "APBD_Task07.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APBD_Task07.App.dll"]
