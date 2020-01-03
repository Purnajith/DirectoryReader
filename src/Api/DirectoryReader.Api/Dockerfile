FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Api/DirectoryReader.Api/DirectoryReader.Api.csproj src/Api/DirectoryReader.Api/
COPY src/Lib/DirectoryReader.Lib/DirectoryReader.Lib.csproj src/Lib/DirectoryReader.Lib/
RUN dotnet restore src/Api/DirectoryReader.Api/DirectoryReader.Api.csproj
COPY . .
WORKDIR /src/src/Api/DirectoryReader.Api
RUN dotnet build DirectoryReader.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DirectoryReader.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DirectoryReader.Api.dll"]
