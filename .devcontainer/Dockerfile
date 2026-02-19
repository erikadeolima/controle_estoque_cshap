FROM mcr.microsoft.com/devcontainers/dotnet:8.0

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

RUN apt-get update \
    && apt-get install -y default-mysql-client \
    && rm -rf /var/lib/apt/lists/*
