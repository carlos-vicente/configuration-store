FROM golang:1.11.1-stretch as builder

WORKDIR /go/src/github.com/carlos-vicente/configuration-store

ADD . /go/src/github.com/carlos-vicente/configuration-store

RUN
    go get -u github.com/govend/govend \
    && govend -v \
    && go build


FROM alpine

COPY --from=builder /go/src/github.com/carlos-vicente/configuration-store/main .

EXPOSE 8080

CMD [ "./main" ]