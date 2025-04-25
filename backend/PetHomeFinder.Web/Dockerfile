FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5259

ENV ASPNETCORE_URLS=http://+:5259

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["PetHomeFinder/backend/src/PetHomeFinder.API/PetHomeFinder.API.csproj", "PetHomeFinder/backend/src/PetHomeFinder.API/"]
RUN dotnet restore "PetHomeFinder/backend/src/PetHomeFinder.API/PetHomeFinder.API.csproj"
COPY . .
WORKDIR "/src/PetHomeFinder/backend/src/PetHomeFinder.API"
RUN dotnet build "PetHomeFinder.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "PetHomeFinder.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetHomeFinder.API.dll"]
