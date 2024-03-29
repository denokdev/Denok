ARG VERSION=5.0

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:${VERSION} AS build

ARG DIST_ENVIRONMENT=development

RUN apt-get update \
    && apt-get install -y build-essential

WORKDIR /source
ENV APP_PATH=/source

COPY .env.${DIST_ENVIRONMENT} ${APP_PATH}/.env

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Denok.Redirector/*.csproj ./Denok.Redirector/
COPY Denok.Lib/*.csproj ./Denok.Lib/
COPY Denok.Web/*.csproj ./Denok.Web/
RUN dotnet restore

# copy everything else and build app
COPY . .

RUN make build

RUN ls

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:${VERSION}

ARG DIST_ENVIRONMENT=development

WORKDIR /denokred
ENV APP_PATH=/denokred

COPY .env.${DIST_ENVIRONMENT} ${APP_PATH}/.env

COPY --from=build /source/denokred ${APP_PATH}/

# ENV ASPNETCORE_URLS http://0.0.0.0:9011

EXPOSE 9011

ENTRYPOINT ["dotnet", "Denok.Redirector.dll"]

# build
# docker build -t denokred .

# run
# docker run --rm --net=denoknet -p 9011:9011 denokred