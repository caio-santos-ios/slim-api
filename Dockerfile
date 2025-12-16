FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Define a variável de ambiente para que a aplicação ouça na porta 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "api-generic.dll"]
