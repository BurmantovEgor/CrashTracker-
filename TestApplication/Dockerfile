FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7027
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CrashTracker.csproj", "./"]
RUN dotnet restore "CrashTracker.csproj"
COPY . .

WORKDIR "/src"
RUN dotnet build "CrashTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CrashTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "CrashTracker.dll"]