FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

FROM build AS publish
COPY ./ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=publish /app/out ./

ENTRYPOINT ["dotnet", "Tasks.Api.dll"]

EXPOSE 80
