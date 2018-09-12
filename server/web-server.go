package main

import "log"
import "net/http"
//import "io/ioutil"
import "github.com/gorilla/mux"
//import "strings"
import "gopkg.in/mgo.v2/bson"
import "encoding/json"
import "strconv"
import "gopkg.in/mgo.v2"
import "fmt"
import "math/rand"
import "time"


// Product represents an e-comm item
type Word struct {
	_IDMongo bson.ObjectId 	     `bson:"_id"`
	NAME string        `json:"name"`
	_ID int32        `json:"id"`
	ANAGRAMS []string `json:"anagrams" bson:"anagrams"`
}
//Counter represents a database entries managers
type Counter struct{
	ID bson.ObjectId	`bson:"_id"`
	NAME string `json:"name"`
	SEQ int32       `json:"seq"`
}
//Score
type Score struct{
	ID bson.ObjectId	`bson:"_id"`
	NAME string `json:"name"`
	SCORE int32       `json:"seq"`
}

var rank []Score


// SERVER the DB server
const SERVER = "localhost:27017"

// DBNAME the name of the DB instance
const DBNAME = "projetolp"

// DOCNAME the name of the document
const DOCWORDS = "words"
const DOCCOUNTER = "counter"
const DOCSCORE = "score"



//GetWord in DB
func GetWord(id int32) Word {
	session, err := mgo.Dial(SERVER)
	if err != nil {
	 fmt.Println("Failed to establish connection to Mongo server:", err)
	}
	defer session.Close()
	c := session.DB(DBNAME).C(DOCWORDS)
	results := Word{}
	if err := c.Find(bson.M{"id": id}).One(&results); err != nil {
	 fmt.Println("Failed to write results:", err)
	}
   return results
}
   
// AddScore inserts an Score in the DB
func AddScore(score Score) bool {
	session, err := mgo.Dial(SERVER)
	defer session.Close()
   
	score.ID = bson.NewObjectId()
	session.DB(DBNAME).C(DOCSCORE).Insert(score)
   
   if err != nil {
	 log.Fatal(err)
	 return false
	}
	return true
}
   
// UpdateCounter updates an counter in the DB
func UpdateCounter(counter Counter) bool {
	session, err := mgo.Dial(SERVER)
	defer session.Close()
	session.DB(DBNAME).C(DOCCOUNTER).UpdateId(counter.NAME, counter)
   
   if err != nil {
	 log.Fatal(err)
	 return false
	}
	return true
}

func GetCounter() int32 {
	var name = "counter"
	session, err := mgo.Dial(SERVER)
	if err != nil {
	 fmt.Println("Failed to establish connection to Mongo server:", err)
	}
	defer session.Close()
	c := session.DB(DBNAME).C(DOCCOUNTER)
	results := Counter{}
	if err := c.Find(bson.M{"name": name}).One(&results); err != nil {
	 fmt.Println("Failed to write results:", err)
	}
   
   return results.SEQ
}



func newGame(w http.ResponseWriter, r *http.Request) {
	var id int32
	var index int32
	index = GetCounter()
	id = rand.Int31n(index)
	word := GetWord(id) // list word
	log.Println(word)
	data, _ := json.Marshal(word)
	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.Header().Set("Access-Control-Allow-Origin", "*")
	w.WriteHeader(http.StatusOK)
	w.Write(data)
	return
}

func createScore(w http.ResponseWriter, r *http.Request) {
	var score Score
	var score2int int
	params := mux.Vars(r)
    _ = json.NewDecoder(r.Body).Decode(&score)
	score.NAME = params["name"]
	score2int, err := strconv.Atoi(params["score"])

   	if err != nil {
      	// handle error
   	}
	score.SCORE = int32(score2int)
    json.NewEncoder(w).Encode(score)
	/*body, err := ioutil.ReadAll(io.LimitReader(r.Body, 1048576)) // read the body of the request
	if err != nil {
		log.Fatalln("Error AddScore", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	if err := r.Body.Close(); err != nil {
		log.Fatalln("Error AddScore", err)
	}
	
	if err := json.Unmarshal(body, &score); err != nil { // unmarshall body contents as a type Candidate
		w.WriteHeader(422) // unprocessable entity
		if err := json.NewEncoder(w).Encode(err); err != nil {
			log.Fatalln("Error AddScore unmarshalling data", err)
			w.WriteHeader(http.StatusInternalServerError)
			return
		}
	}*/
	success := AddScore(score) // adds the album to the DB
	if !success {
		w.WriteHeader(http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json; charset=UTF-8")
	w.WriteHeader(http.StatusCreated)
	return
	//fmt.Fprintln(w, "not implemented yet !")
}



func main() {
	rand.Seed(time.Now().UnixNano())
	r := mux.NewRouter()
	r.HandleFunc("/newgame", newGame).Methods("GET")
	r.HandleFunc("/newscore/{name}/{score}", createScore).Methods("POST")
	if err := http.ListenAndServe(":3000", r); err != nil {
		log.Fatal(err)
	}
}