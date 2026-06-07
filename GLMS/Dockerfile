FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore GLMS/GLMS.csproj

RUN dotnet publish GLMS/GLMS.csproj \
    -c Release \
    -o /app/publish

#
# RUNTIME
#

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GLMS.dll"]