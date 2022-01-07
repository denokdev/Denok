FROM rust:1.50 as build

WORKDIR /source

# copy over your manifests
COPY denok-redirector-rs/ .

RUN cargo build --release

# final base
FROM debian:buster-slim

ARG DIST_ENVIRONMENT=development

WORKDIR /denokred
ENV APP_PATH=/denokred

COPY .env.${DIST_ENVIRONMENT} ${APP_PATH}/.env

# copy the build artifact from the build stage
COPY --from=build /source/target/release/redirector ${APP_PATH}/

EXPOSE 9011

# set the startup command to run your binary
CMD ["./redirector"]