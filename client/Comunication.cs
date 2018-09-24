using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Comunication{
    public class Words{	//Modelo do Json de palavras
        public string Name { get; set; }
        public List<string> Anagrams { get; set; }
    }
    public class Rank{	//Modelo do Json de rank 
        public List<string> Name { get; set; }
        public List<int> points { get; set; }
    }

	class Http
	{
         StreamReader reader;
         Stream dataStream;
         WebResponse response;
         WebRequest request;
         Words words;
         Rank rank;
		public Words newGame(){
            try{
            request = WebRequest.Create("http://localhost:3000/newgame");
            response = request.GetResponse();
            dataStream = response.GetResponseStream(); 
            reader = new StreamReader(dataStream);  
            //Read the content
            string responseFromServer = reader.ReadToEnd();

            //Converte a String do request em json
            words = Newtonsoft.Json.JsonConvert.DeserializeObject<Words>(responseFromServer);
            
            }catch(Exception e){
                Console.WriteLine("Erro ao iniciar!");
                Console.WriteLine("Source :{0} " , e.Source);
                Console.WriteLine("Message :{0} " , e.Message);
            }
            return words;
		}

        public Rank getRank(){
            try{
            request = WebRequest.Create("http://localhost:3000/rank");
            response = request.GetResponse();
            dataStream = response.GetResponseStream(); 
            reader = new StreamReader(dataStream);  
            //Read the content
            string responseFromServer = reader.ReadToEnd();

            //Converte a String do request em json
            rank = Newtonsoft.Json.JsonConvert.DeserializeObject<Rank>(responseFromServer);
            
            
            
            }catch(Exception e){
                Console.WriteLine("Erro ao obter o rank!");
                Console.WriteLine("Message :{0} " , e.Message);
            }
            return rank;
        }

        public bool saveScore(String name, int score){
            String link = "http://localhost:3000/newscore/"+name+"/"+score;
            try{
            request = WebRequest.Create(link);
            
            
            }catch(Exception e){
                Console.WriteLine("Erro ao salvar");
                Console.WriteLine("Message :{0} " , e.Message);
                return false;
            }
            return true;
        }
        
        public bool disconnect(){
            try{
                reader.Close ();
                dataStream.Close ();
                response.Close ();
            }catch(Exception e){
                Console.WriteLine("Erro ao sair");
                Console.WriteLine("Message :{0} " , e.Message);
                return false;
            }
            return true;
        }

        public static void Main(String[] args){

        }

	}
}  