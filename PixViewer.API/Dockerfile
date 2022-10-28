#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PixViewer.API/PixViewer.API.csproj", "PixViewer.API/"]
RUN dotnet restore "PixViewer.API/PixViewer.API.csproj"
COPY . .
WORKDIR "/src/PixViewer.API"
RUN dotnet build "PixViewer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PixViewer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PixViewer.API.dll"]