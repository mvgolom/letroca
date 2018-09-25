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
        int score;
        string name;
        int qtdWords = 2;
        bool win = false;

        public Logic(){
            playBackgroundSound();

            Console.WriteLine("\n-#-#-#-#- LETROCA -#-#-#-#-");
            Console.WriteLine("\n---- FASE 1 ----");
            
            init();
            play();

            if(win){
                Console.WriteLine("\nPASSOU PARA A PRÓXIMA FASE");
                Console.WriteLine("\n---- FASE 2 ----");
                qtdWords = 4;
                init();
                play();
            }

            
            if(win){
                Console.WriteLine("\nPASSOU PARA A PRÓXIMA FASE");
                Console.WriteLine("\n---- FASE 3 ----");
                qtdWords = 8;
                init();
                play();
            }

            
            if(win){
                Console.WriteLine("\nPASSOU PARA A ÚLTIMA FASE");
                Console.WriteLine("\n---- FASE FINAL  ----");
                qtdWords = 16;
                init();
                play();
            }

            if(win){
                Console.WriteLine("\n\nPARABÉNS!! VOCÊ VENCEU!! XD \nFez "+this.score+" pontos.");
                Console.Write("Digite o seu nome, campeão: ");
                name  = Console.ReadLine();

            }

            http.saveScore(name, score);

			Console.WriteLine("Pontos salvos");
            Console.WriteLine("Obtendo o rank...");
            try{
                printRank(); 
            }catch{
        }
        }

        public void init(){
            
            
            Console.WriteLine("\nbuscando palavras...");
            http = new Http();

            Words w = http.newGame();

            //letters
            letters = w.Name;

            //words
            List<string> anagrams = w.Anagrams;
            Random rnd = new Random();


            for(int i = 0; i < qtdWords; i++){
                int x = rnd.Next(anagrams.Count);
                words.Add(anagrams[x]);
            }

            this.score = 0;
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
              //  p.StartInfo.CreateNoWindow = true;
               // p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; 
                p.StartInfo.FileName = " ";
                p.StartInfo.Arguments = "play marshmello-alone.wav";
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
                    this.score = this.score - 10;
                    break;
                }

                if(guessWord.Equals("?")){// pediu dica
                    Console.WriteLine(tips());   
                    continue;                 
                }

                if(guessWord.Equals("??")){// pediu resposta embaralhada
                    Console.WriteLine(shuffleAnswer());   
                    this.score = this.score - 5;
                    continue;                 
                }

				for(int i = 0; i < words.Count; i++){ // verifica se palavra existe
                    string su = words[i].ToUpper();
					if(su.Equals(guessWord.ToUpper())){
						flag = true;
                        words.RemoveAt(i);
					}
				}
                             
				if(flag){// verifica se acertou
					Console.WriteLine("--> ACERTOU");
					this.score = this.score + 10;
				}else{
					Console.WriteLine("--> Erroooou. Palavra inexistente ou não cadastrada");
                    Console.WriteLine("\nVocê pode:");
                    Console.WriteLine("- Rever as dicas digitando \"?\" ");
                    Console.WriteLine("- Pedir as respostas embaralhadas digitando \"??\"(perde 5 pontos) ");
                    Console.WriteLine("- Desistir do jogo digitando \"--\"(perde 10 pontos)");
				}

            }
            string name ;

            if(on){// se não desistiu
                this.score = this.score + 15;
                win = true;
            }else{
                Console.WriteLine("\n\nVOCÊ DESISTIU :( \n Mas ainda fez "+this.score+" pontos :)");
			    Console.Write("Digite seu nome: ");
                name = Console.ReadLine();
                win = false;
            }

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
            
            }catch{
                Console.WriteLine("Erro ao iniciar. Verifique a conexão.");

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
            
            
            }catch{
                Console.WriteLine("Erro ao salvar. Verifique a conexão");
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
