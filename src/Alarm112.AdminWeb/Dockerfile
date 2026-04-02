FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/Alarm112.AdminWeb/Alarm112.AdminWeb.csproj src/Alarm112.AdminWeb/
RUN dotnet restore src/Alarm112.AdminWeb/Alarm112.AdminWeb.csproj
COPY src/Alarm112.AdminWeb/ src/Alarm112.AdminWeb/
RUN dotnet publish src/Alarm112.AdminWeb/Alarm112.AdminWeb.csproj -c Release -o /app/out

FROM base AS final
WORKDIR /app
COPY --from=build /app/out .
# Run as non-root
USER appuser
ENTRYPOINT ["dotnet", "Alarm112.AdminWeb.dll"]
