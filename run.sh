#! /bin/bash
docker build -t dotnet-core-api ./
docker run -d -p 80:80 --name dotnet-core-api --env APP_SECRET_NAME=my_app_secret_name dotnet-core-api
echo "http://localhost/health"
echo "http://localhost/secret"