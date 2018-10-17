FROM golang:1.11.1-alpine3.8 as builder

WORKDIR /go/src/github.com/carlos-vicente/configuration-store

ADD . /go/src/github.com/carlos-vicente/configuration-store

RUN
    go get -u github.com/gpmgo/gopm \
    && gopm get \
    && gopm build


FROM alpine

COPY --from=builder /go/src/github.com/carlos-vicente/configuration-store/.vendor/bin .

EXPOSE 8080

CMD [ "./configuration-store" ]