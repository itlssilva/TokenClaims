FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY ["Consumer/TokenClaimConsumer/TokenClaimConsumer.Api/TokenClaimConsumer.Api.csproj", "Consumer/TokenClaimConsumer/TokenClaimConsumer.Api/"]
COPY ["Generate/TokenClaimProvider/TokenClaimProvider.Api/TokenClaimProvider.Api.csproj", "Generate/TokenClaimProvider/TokenClaimProvider.Api/"]

RUN dotnet restore "Consumer/TokenClaimConsumer/TokenClaimConsumer.Api/TokenClaimConsumer.Api.csproj"
RUN dotnet restore "Generate/TokenClaimProvider/TokenClaimProvider.Api/TokenClaimProvider.Api.csproj"

COPY . .
WORKDIR "/src/Consumer/TokenClaimConsumer/TokenClaimConsumer.Api"
RUN dotnet build "TokenClaimConsumer.Api.csproj" -c Release -o /app/consumer --no-restore

WORKDIR "/src/Generate/TokenClaimProvider/TokenClaimProvider.Api"
RUN dotnet build "TokenClaimProvider.Api.csproj" -c Release -o /app/generate --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/consumer .
COPY --from=build /app/generate .
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT [ "./entrypoint.sh" ]
