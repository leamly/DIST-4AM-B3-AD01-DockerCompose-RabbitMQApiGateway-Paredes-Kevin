FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY ["InventarioA.Api.csproj", "./"]

RUN dotnet restore "InventarioA.Api.csproj"
COPY . .
RUN dotnet build "InventarioA.Api.csproj" -c Release -o /app/publish /p:UseAppHost=False

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "InventarioA.Api.dll"]
