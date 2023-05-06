# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "MicroServiceProxy.csproj"
RUN dotnet publish "MicroServiceProxy.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
ENV DOTNET_URLS=http://+:5002
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5002

ENTRYPOINT ["dotnet", "MicroServiceProxy.dll"]
