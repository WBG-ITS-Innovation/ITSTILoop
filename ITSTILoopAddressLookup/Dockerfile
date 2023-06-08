#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ITSTILoopAddressLookup/ITSTILoopAddressLookup.csproj", "ITSTILoopAddressLookup/"]
COPY ["ITSTILoopDTOLibrary/ITSTILoopLibrary.csproj", "ITSTILoopDTOLibrary/"]
RUN dotnet restore "ITSTILoopAddressLookup/ITSTILoopAddressLookup.csproj"
COPY . .
WORKDIR "/src/ITSTILoopAddressLookup"
RUN dotnet build "ITSTILoopAddressLookup.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ITSTILoopAddressLookup.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ITSTILoopAddressLookup.dll"]