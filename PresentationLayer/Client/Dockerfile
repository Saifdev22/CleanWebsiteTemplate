FROM mcr.microsoft.com/dotnet/core/sdk:8.0 AS build-env
WORKDIR /app

COPY Client/Client.csproj Client/
COPY Shared/Shared.csproj Shared/

RUN dotnet restore Client/Client.csproj
COPY . .
RUN dotnet publish -c Release -o output

FROM nginx:alpine
WORKDIR /app
COPY --from=build-env /app/output/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80