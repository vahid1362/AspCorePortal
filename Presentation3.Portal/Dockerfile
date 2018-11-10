#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 57140
EXPOSE 44349

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Presentation3.Portal/Presentation3.Portal.csproj", "Presentation3.Portal/"]
RUN dotnet restore "Presentation3.Portal/Presentation3.Portal.csproj"
COPY . .
WORKDIR "/src/Presentation3.Portal"
RUN dotnet build "Presentation3.Portal.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Presentation3.Portal.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Presentation3.Portal.dll"]