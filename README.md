**Docker build steps**

build images:
docker compose build

run Development image:
docker compose -f ./docker-compose.Development.yml up

after this you can view Swagger at port 5000:

http://localhost:5000/swagger/index.html

Development container also creates local database so you can add any data you want without any risk

run Production image:
docker compose -f ./docker-compose.yml up

Production server does not allows you to access Swagger, but it connects to remote Mongo db with production data

you also can access this with port 5000:

http://localhost:5000/