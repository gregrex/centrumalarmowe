FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
RUN apt-get update \
    && apt-get install -y --no-install-recommends wget \
    && rm -rf /var/lib/apt/lists/*
RUN groupadd --system appgroup && useradd --system --gid appgroup --create-home --home-dir /home/appuser appuser
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY Directory.Build.props ./
COPY src/Alarm112.AdminWeb/Alarm112.AdminWeb.csproj src/Alarm112.AdminWeb/
COPY src/Alarm112.Contracts/Alarm112.Contracts.csproj src/Alarm112.Contracts/
RUN dotnet restore src/Alarm112.AdminWeb/Alarm112.AdminWeb.csproj
COPY src/ src/
RUN dotnet publish src/Alarm112.AdminWeb/Alarm112.AdminWeb.csproj -c Release -o /app/out --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/out .
# Run as non-root
USER appuser
ENTRYPOINT ["dotnet", "Alarm112.AdminWeb.dll"]
