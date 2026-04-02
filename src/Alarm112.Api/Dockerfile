FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/Alarm112.Api/Alarm112.Api.csproj src/Alarm112.Api/
COPY src/Alarm112.Contracts/Alarm112.Contracts.csproj src/Alarm112.Contracts/
COPY src/Alarm112.Application/Alarm112.Application.csproj src/Alarm112.Application/
COPY src/Alarm112.Infrastructure/Alarm112.Infrastructure.csproj src/Alarm112.Infrastructure/
COPY src/Alarm112.Domain/Alarm112.Domain.csproj src/Alarm112.Domain/
RUN dotnet restore src/Alarm112.Api/Alarm112.Api.csproj
COPY src/ src/
RUN dotnet publish src/Alarm112.Api/Alarm112.Api.csproj -c Release -o /app/out --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/out .
# Copy content data bundles into the image
COPY data/ /app/data/
ENV ContentBundles__DataRoot=/app/data
# Run as non-root
USER appuser
ENTRYPOINT ["dotnet", "Alarm112.Api.dll"]
