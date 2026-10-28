FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY Directory.Build.props ./
COPY 02-enterprise-dotnet-portal/src/ ./02-enterprise-dotnet-portal/src/
RUN dotnet publish 02-enterprise-dotnet-portal/src/EnterprisePortal.Web -c Release -o /app
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "EnterprisePortal.Web.dll"]
