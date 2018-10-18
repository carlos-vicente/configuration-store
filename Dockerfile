FROM golang:1.11.1-stretch as builder

WORKDIR /workspace/src/configuration-store

ADD . /workspace/src/configuration-store

ENV GOPATH="$GOPATH:/workspace"

RUN
    go get -u github.com/golang/dep/cmd/dep \
    && dep ensure \
    && go build


FROM alpine

COPY --from=builder /workspace/src/configuration-store/configuration-store .

EXPOSE 8080

CMD [ "./configuration-store" ]