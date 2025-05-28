FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore "Presenation/API/API.csproj"

RUN dotnet build "Presenation/API/API.csproj" -c Release -o /app/build

RUN dotnet publish "Presenation/API/API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
