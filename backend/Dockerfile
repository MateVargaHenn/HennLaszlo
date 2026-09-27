FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore src/WebApi/WebApi.csproj
RUN dotnet publish src/WebApi/WebApi.csproj -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
RUN mkdir -p /app/storage /home/app/.aspnet/DataProtection-Keys \
    && chown -R 1654:1654 /app/storage /home/app/.aspnet
USER 1654
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebApi.dll"]
