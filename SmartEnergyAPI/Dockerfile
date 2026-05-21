FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["SmartEnergyAPI/SmartEnergyAPI.csproj", "SmartEnergyAPI/"]
RUN dotnet restore "SmartEnergyAPI/SmartEnergyAPI.csproj"
COPY . .
WORKDIR "/src/SmartEnergyAPI"
RUN dotnet build "SmartEnergyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartEnergyAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartEnergyAPI.dll"]