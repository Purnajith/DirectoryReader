FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Web/DirectoryReader.Webapp/DirectoryReader.Webapp.csproj src/Web/DirectoryReader.Webapp/
COPY src/Lib/DirectoryReader.Lib/DirectoryReader.Lib.csproj src/Lib/DirectoryReader.Lib/
RUN dotnet restore src/Web/DirectoryReader.Webapp/DirectoryReader.Webapp.csproj
COPY . .
WORKDIR /src/src/Web/DirectoryReader.Webapp
RUN dotnet build DirectoryReader.Webapp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DirectoryReader.Webapp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DirectoryReader.Webapp.dll"]
