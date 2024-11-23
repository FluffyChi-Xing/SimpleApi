FROM ubuntu:22.04
RUN apt update && apt install -y git build-essential libssl-dev
WORKDIR /app
COPY . .
WORKDIR /app/cangjie_gateway
RUN /bin/bash -c ". /app/cangjie/envsetup.sh;cjpm clean;cjpm update;cjpm build"
RUN chmod +x entrypoint.sh
ENTRYPOINT ["/app/cangjie_gateway/entrypoint.sh"]