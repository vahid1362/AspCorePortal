FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY Portal.Presentation/Portal.Presentation.csproj Portal.Presentation/
RUN dotnet restore Portal.Presentation/Portal.Presentation.csproj
COPY . .
WORKDIR /src/Portal.Presentation
RUN dotnet build Portal.Presentation.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Portal.Presentation.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Portal.Presentation.dll"]
