using System;
using System.Diagnostics;
using System.Collections.Generic;


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
        Comunication.Http http;
        char[] letters;
        List<string> words = new List<string>();
        int points;

        public Logic(){
            init();
            play();
        }

        public void init(){

            http = new Comunication();

            Http.Words w = http.newGame();

            //letters
            string word = w.Name;

            letters = new char[word.Length];
            for(int i = 0; i < word.Length; i++){
                letters[i] = (char) word.Substring(i);
            }

            //words
            List<string> anagrams = w.Anagrams;
            Random rnd = new Random();


            for(int i = 0; i < 5; i++){
                int x = rnd.Next(anagrams.Length());
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

			while (words.Count() > 0){//enquanto não vencer
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

            Comunication.Rank r = http.getRank();
            Console.WriteLine(r);


        }



	}
}
