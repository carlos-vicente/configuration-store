# build go app
FROM golang:1.11.2-stretch as appBuilder

WORKDIR /workspace/src/configuration-store

ADD . /workspace/src/configuration-store

ENV GOPATH="$GOPATH:/workspace"

RUN \
    go get -u github.com/golang/dep/cmd/dep \
    && dep ensure -vendor-only \
    && go test \
    && CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o /workspace/src/configuration-store/output/config-store .


# build react app
FROM node:8.12.0-alpine as webBuilder

ADD ./web /web

WORKDIR /web

RUN npm install \
    && ./node_modules/.bin/yarn install \
    && ./node_modules/.bin/gulp


# bundle app
FROM alpine

RUN apk add --no-cache openssl

ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-alpine-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-alpine-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-alpine-linux-amd64-$DOCKERIZE_VERSION.tar.gz

COPY --from=appBuilder /workspace/src/configuration-store/output/config-store /app/config-store
COPY --from=webBuilder /web/views /app/web/views
COPY --from=webBuilder /web/dist /app/web/dist

EXPOSE 8888

ENTRYPOINT ["/app/config-store"]
CMD ["--port=8888"]
