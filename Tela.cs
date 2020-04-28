using System;
using tabuleiro;

namespace xadrez_console
{
    public class Tela
    {
        private const int linhasI = 8;
        private const string colunasI = "a b c d e f g h";
        public static void imprimirTabuleiro(Tabuleiro tab){

            for (int i = 0; i < tab.linhas; i++)
            {
                Console.Write(linhasI -i + " ");
                for (int j = 0; j < tab.colunas; j++)
                {
                    if(tab.peca(i,j) == null){
                        Console.Write("-" + " ");
                    }else{
                        imprimirPeca(tab.peca(i,j));
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("  " + colunasI);
        }

        public static void imprimirPeca(Peca peca){
            if (peca.cor == Cor.Branca)
                Console.Write(peca);
            else{
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(peca);
                Console.ForegroundColor = aux;
            }
        }
    }
}
