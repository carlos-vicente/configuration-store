FROM golang:1.11.1-alpine3.8 as builder

WORKDIR /go/src/github.com/carlos-vicente/configuration-store

ADD . /go/src/github.com/carlos-vicente/configuration-store

RUN gopm get
    && go build


FROM alpine

COPY --from=builder /go/src/github.com/carlos-vicente/configuration-store/main .

EXPOSE 8080

CMD [ "./main" ]