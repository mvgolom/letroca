using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;

namespace Game
{
    class main{
        public static void Main(String[] args){
            Logic logic =  new Logic();
        }
    }
	class Logic
	{
        Http http;
        string letters;
        List<string> words = new List<string>();
        int points;

        public Logic(){
            init();
            play();
        }

        public void init(){
            playBackgroundSound();

            Console.WriteLine("----- LETROCA -----");
            
            Console.WriteLine("\nbuscando palavras...");
            http = new Http();

            Words w = http.newGame();

            //letters
            letters = w.Name;

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
            new Thread(() => 
            {
                Thread.CurrentThread.IsBackground = true; 
                /* run your code here */ 
                
                // Start the child process.
                Process p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.RedirectStandardOutput = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; 
                p.StartInfo.FileName = "play ";
                p.StartInfo.Arguments = "marshmello-alone.wav";
                p.Start();

            }).Start();

            
        }

        public string tips(){
            // 3 palavras de 2 letras. Iniciais: A, E, R,
            // 4 palavras de 3 letras. Iniciais: L, A, I, R,
            // 2 palavras de 4 letras. Iniciais: E, I,
            string dica = "\n\nDicas\n";
            for(int i = 2; i <= letters.Length; i++){
                int countWords = 0;
                string iniciais = "";
                foreach(string s in words){
                   if(s.Length == i){
                        countWords += 1;
                        iniciais += s.Substring(0,1) + ", ";
                   }
                }
                if(countWords > 0){
                    dica += countWords+" palavras de "+i+" letras. Iniciais: "+iniciais.ToUpper()+"\n";
                }
            }

            return dica;
        }
        public string ScrambleWord(string word) 
        { 
            char[] chars = new char[word.Length]; 
            Random rand = new Random(10000); 
            int index = 0; 
            while (word.Length > 0) 
            { // Get a random number between 0 and the length of the word. 
                int next = rand.Next(0, word.Length - 1); // Take the character from the random position 
                                                        //and add to our char array. 
                chars[index] = word[next];                // Remove the character from the word. 
                word = word.Substring(0, next) + word.Substring(next + 1); 
                ++index; 
            } 
            return new String(chars); 
        }  

        public string shuffleAnswer(){
            string r = "\n\nRespostas Embaralhadas\n";
            foreach(string str in words){
                r += ScrambleWord(str)+"\n";
            }
            return r;
        }

        public void printRank(){
            Rank r = http.getRank();
            List<string> n = r.Name;
            List<int> p = r.points;
            for(int i = 0; i < n.Count; i++){
                Console.WriteLine(n[i]+": "+p[i]+" pts");
            }
        }

		public void play()
		{                    
			Console.Write("\nLetras: ");

			Console.WriteLine(ScrambleWord(letters).ToUpper());
            Console.WriteLine(tips()); 

            Boolean on = true; // false = desistiu do jogo

			while (words.Count > 0){//enquanto não vencer
				Console.Write("\nDigite uma Palavra:");
				string guessWord = Console.ReadLine();
                Boolean flag = false;//se acertou ou não

                if(guessWord.Equals("--")){// verifica se quer desistir
                    on = false;
                    this.points = this.points - 15;
                    break;
                }

                if(guessWord.Equals("?")){// pediu dica
                    Console.WriteLine(tips());   
                    continue;                 
                }

                if(guessWord.Equals("??")){// pediu resposta embaralhada
                    Console.WriteLine(shuffleAnswer());   
                    this.points = this.points - 7;
                    continue;                 
                }


				foreach(String s in words){ // verifica se palavra existe
                    string su = s.ToUpper();
					if(su.Equals(guessWord.ToUpper())){
						flag = true;
                        words.Remove(s);
					}
				}
                             
				if(flag){// verifica se acertou
					Console.Write("--> ACERTOU");
					this.points = this.points + 10;
				}else{
					Console.Write("--> Erroooou. Palavra inexistente ou não cadastrada");
                    Console.WriteLine("\n(vc pode pedir dicas inserindo \"?\", pedir as respostas embaralhadas inserindo \"??\"(perde ponto) ou desistir do jogo inserindo \"--\"(perde mais pontos))");
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
            Console.WriteLine("Obtendo o rank...");
            try{
                printRank(); 
            }catch{

            }

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
            request = WebRequest.Create("https://guarded-reaches-35788.herokuapp.com/newgame");
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
            request = WebRequest.Create("https://guarded-reaches-35788.herokuapp.com/rank");
            response = request.GetResponse();
            dataStream = response.GetResponseStream(); 
            reader = new StreamReader(dataStream);  
            //Read the content
            string responseFromServer = reader.ReadToEnd();

            //Converte a String do request em json
            rank = Newtonsoft.Json.JsonConvert.DeserializeObject<Rank>(responseFromServer);
            
            
            
            }catch{
                Console.WriteLine("Erro ao obter o rank!");
            }
            return rank;
        }

        public bool saveScore(String name, int score){
            String link = "https://guarded-reaches-35788.herokuapp.com/newscore/"+name+"/"+score;
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
