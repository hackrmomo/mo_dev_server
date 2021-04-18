FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /modev-server
EXPOSE 5001
EXPOSE 5000
COPY ./MoDev.Server/bin/Release/net5.0/publish ./

ENTRYPOINT ["dotnet", "MoDev.Server.dll"]