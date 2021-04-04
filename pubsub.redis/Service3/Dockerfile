#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Service3/Service3.csproj", "Service3/"]
RUN dotnet restore "Service3/Service3.csproj"
COPY . .
WORKDIR "/src/Service3"
RUN dotnet build "Service3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service3.dll"]