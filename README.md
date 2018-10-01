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

# Client C#
* [Install Dotnet](https://www.microsoft.com/net/download/linux-package-manager/ubuntu16-04/sdk-current)
    * [Dotnet Install Documentation](https://www.microsoft.com/net/download/linux-package-manager/ubuntu16-04/sdk-current)

* [Run Client](https://docs.microsoft.com/pt-br/dotnet/core/tutorials/using-with-xplat-cli)
    ```sh
    dotnet run
    ```
## Dependencias
* [Newtonsoft.Json](https://www.newtonsoft.com/json)
    ```sh
    dotnet add  package Newtonsoft.Json
    ```
* [RestSharp](http://restsharp.org/)
    ```sh
    dotnet add package RestSharp
    ```
* [soX](http://manpages.ubuntu.com/manpages/bionic/man1/sox.1.html)
    ```sh
    sudo apt-get install sox
    ```
* [soX mp3-lib](https://packages.debian.org/source/sid/sox)
    ```sh
    sudo apt-get install libsox-fmt-mp3
    ```
* [Xterm](https://invisible-island.net/xterm/)
    ```sh
    sudo apt install xterm
    ```
