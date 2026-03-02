# Use the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["GovContracts.Web.csproj", "."]
RUN dotnet restore "GovContracts.Web.csproj"
COPY . .
RUN dotnet build "GovContracts.Web.csproj" -c Release -o /app/build
RUN dotnet publish "GovContracts.Web.csproj" -c Release -o /app/publish

# Use Nginx to host the static Blazor WASM content
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
