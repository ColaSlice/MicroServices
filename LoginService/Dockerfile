# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "LoginService.csproj"
RUN dotnet publish "LoginService.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
ENV DOTNET_URLS=http://+:5001
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5001

ENTRYPOINT ["dotnet", "LoginService.dll"]
