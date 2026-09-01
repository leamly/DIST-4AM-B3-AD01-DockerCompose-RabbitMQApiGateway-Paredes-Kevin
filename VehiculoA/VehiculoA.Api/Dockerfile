FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY ["VehiculoA.Api.csproj", "./"]

RUN dotnet restore "VehiculoA.Api.csproj"
COPY . .
RUN dotnet build "VehiculoA.Api.csproj" -c Release -o /app/publish /p:UseAppHost=False

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "VehiculoA.Api.dll"]
