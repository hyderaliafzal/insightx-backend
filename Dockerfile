#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ConnektaViz.API.csproj", "ConnektaViz.Api/"]
RUN dotnet restore "ConnektaViz.Api/ConnektaViz.API.csproj"

WORKDIR "/src/ConnektaViz.Api"
COPY . .
RUN dotnet build "ConnektaViz.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConnektaViz.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConnektaViz.API.dll"]

# Apply database migrations
FROM base AS migrate
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConnektaViz.API.dll"]

CMD ["dotnet", "ef", "database", "update"]