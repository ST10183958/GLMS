FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore GLMS.Api/GLMS.Api.csproj

RUN dotnet publish GLMS.Api/GLMS.Api.csproj \
    -c Release \
    -o /app/publish

#
# RUNTIME
#

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GLMS.Api.dll"]