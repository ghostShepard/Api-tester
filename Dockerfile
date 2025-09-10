# 使用官方 .NET SDK 镜像编译项目
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ApiTesterWeb.csproj", "./"]
RUN dotnet restore "./ApiTesterWeb.csproj"

COPY . .
RUN dotnet publish "ApiTesterWeb.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5035

ENTRYPOINT ["dotnet", "ApiTesterWeb.dll", "--urls", "http://0.0.0.0:5035"]
