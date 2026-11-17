
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ExpenseTracker.csproj", "./"]
RUN dotnet restore "ExpenseTracker.csproj"

COPY . .
RUN dotnet publish "ExpenseTracker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExpenseTracker.dll"]
