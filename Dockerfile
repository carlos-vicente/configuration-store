FROM golang:1.11.1-stretch as builder

WORKDIR /workspace/src/configuration-store

ADD . /workspace/src/configuration-store

ENV GOPATH="$GOPATH:/workspace"

RUN
    go get -u github.com/gpmgo/gopm \
    && gopm get -v \
    && go build


FROM alpine

COPY --from=builder /workspace/src/configuration-store/configuration-store .

EXPOSE 8080

CMD [ "./configuration-store" ]