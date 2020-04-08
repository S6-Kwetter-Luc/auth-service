FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.3-bionic

WORKDIR /app
COPY /*.dll .

ENTRYPOINT ["dotnet", "*.dll"]