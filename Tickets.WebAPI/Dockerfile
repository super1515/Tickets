FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tickets.WebAPI/Tickets.WebAPI.csproj", "Tickets.WebAPI/"]
COPY ["Tickets.BAL/Tickets.BAL.csproj", "Tickets.BAL/"]
COPY ["Tickets.DAL/Tickets.DAL.csproj", "Tickets.DAL/"]
RUN dotnet restore "Tickets.WebAPI/Tickets.WebAPI.csproj"
COPY . .
WORKDIR "/src/Tickets.WebAPI"
RUN dotnet build "Tickets.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tickets.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tickets.WebAPI.dll"]