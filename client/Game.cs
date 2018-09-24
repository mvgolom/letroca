using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game
{
    class main{
        public static void Main(String[] args){
            Logic logic =  new Logic();
        }
    }
	class Logic
	{
        //private char[] letters =  { 'O', 'R', 'T', 'O', 'S' };
        //private String[] words  = { "ROTO", "TORO", "ROSTO", "TOROS" };
        //private int qtt;
        Http http;
        char[] letters;
        List<string> words = new List<string>();
        int points;

        public Logic(){
            init();
            play();
        }

        public void init(){

            http = new Http();

            Words w = http.newGame();

            //letters
            string word = w.Name;

            letters = word.ToCharArray();
    

            //words
            List<string> anagrams = w.Anagrams;
            Random rnd = new Random();


            for(int i = 0; i < 5; i++){
                int x = rnd.Next(anagrams.Count);
                words.Add(anagrams[x]);
            }



            this.points = 0;
        }

        public void playBackgroundSound(){
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "play";
            p.StartInfo.Arguments = "marshmello-alone.wav";
            p.Start();
        }

		public void play()
		{

            playBackgroundSound();
                    
			Console.WriteLine("LETROCA");
			Console.Write("Letras: ");

			foreach (char l in letters){//imprime as letras
                Console.Write(l + " ");
            }

            Boolean on = true; // false = desistiu do jogo

			while (words.Count > 0){//enquanto não vencer
				Console.Write("\nDigite uma Palavra:");
				string guessWord = Console.ReadLine();
                Boolean flag = false;//se acertou ou não

                if(guessWord.Equals("--")){// verifica se quer desistir
                    on = false;
                    this.points = this.points - 10;
                    break;
                }

				foreach(String s in words){ // verifica se palavra existe
					if(s.Equals(guessWord.ToUpper())){
						flag = true;
                        words.Remove(s);
					}
				}
                             
				if(flag){// verifica se acertou
					Console.Write("--> ACERTOU");
					this.points = this.points + 10;
				}else{
					Console.Write("--> Erroooou. Palavra inexistente ou não cadastrada");
                    Console.WriteLine("\n(vc pode desistir do jogo a qualquer momento inserindo \"--\" sem as aspas)");
				}

				

            }
            string name ;

            if(on){// se não desistiu
                Console.WriteLine("\n\nVOCÊ VENCEU!! Fez "+this.points+" pontos.");
			    Console.Write("Digite o seu nome, campeão: ");
                name  = Console.ReadLine();
            }else{
                Console.WriteLine("\n\nVOCÊ DESISTIU!! \n Mas ainda fez "+this.points+" pontos.");
			    Console.Write("Digita logo esse nome, vai: ");
                name = Console.ReadLine();
            }

            
			

            http.saveScore(name, points);

			Console.WriteLine("Pontos salvos");

            Rank r = http.getRank();
            Console.WriteLine(r);


        }


	}

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

	}
}
