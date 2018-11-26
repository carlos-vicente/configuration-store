# build go app
FROM golang:1.11.2-stretch as appBuilder

WORKDIR /workspace/src/configuration-store

ADD . /workspace/src/configuration-store

ENV GOPATH="$GOPATH:/workspace"

RUN \
    go get -u github.com/golang/dep/cmd/dep \
    && dep ensure -vendor-only \
    && go test \
    && CGO_ENABLED=0 GOOS=linux go build -a -installsuffix -o /workspace/src/configuration-store/output/config-store .


# build react app
FROM node:8.12.0-alpine as webBuilder

ADD ./web /web

WORKDIR /web

RUN npm install \
    && ./node_modules/.bin/yarn install \
    && ./node_modules/.bin/gulp


# bundle app
FROM alpine

COPY --from=appBuilder /workspace/src/configuration-store/output/config-store /app/config-store
COPY --from=webBuilder /web/views /app/web/views
COPY --from=webBuilder /web/dist /app/web/dist

EXPOSE 8888

WORKDIR /app

ENTRYPOINT ["/app/config-store"]
CMD ["8888"]