FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.3-bionic

WORKDIR /app
COPY /* ./

ENTRYPOINT ["dotnet", "auth-service.dll"]