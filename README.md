# Servidor GO
* [Run Server](https://golang.org/cmd/go/)
    ```sh
    go run web-server.go
    ```
* [Compile Server and Dependencies](https://golang.org/cmd/go/)
    ```sh
    go build web-server.go
    ```
* [Server Test](#servidor-go)
    ```sh
    curl http://localhost:3000/newgame
    ```
## Dependencias
* [github.com/gorilla/mux](https://github.com/gorilla/mux)
    ```sh
    go get -u github.com/gorilla/mux
    ```
* [gopkg.in/mgo.v2](https://github.com/go-mgo/mgo)
    ```sh
    go get gopkg.in/mgo.v2
    ```
# MongoDB
* [Install MongoDB](https://docs.mongodb.com/manual/tutorial/install-mongodb-on-ubuntu)    
    ```sh
    sudo service mongod start
    ```
* [Import Project Database](https://docs.mongodb.com/manual/reference/program/mongorestore/index.html)
    ```sh
    mongorestore --db projetolp extract_place_dump.zip/dump/projetolp
    ```

# Client