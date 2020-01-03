FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Identity/DirectoryReader.Identity/DirectoryReader.Identity.csproj src/Identity/DirectoryReader.Identity/
COPY src/Lib/DirectoryReader.Lib/DirectoryReader.Lib.csproj src/Lib/DirectoryReader.Lib/
RUN dotnet restore src/Identity/DirectoryReader.Identity/DirectoryReader.Identity.csproj
COPY . .
WORKDIR /src/src/Identity/DirectoryReader.Identity
RUN dotnet build DirectoryReader.Identity.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DirectoryReader.Identity.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DirectoryReader.Identity.dll"]
