using System;
using tabuleiro;
using xadrez;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                PartidaXadrez partida = new PartidaXadrez();
                

           

            // PosicaoXadrez posicaoXadrez = new PosicaoXadrez('c', 7);
            // Console.WriteLine(posicaoXadrez.toPosicao());
            Tela.imprimirTabuleiro(partida.tabuleiro);
            }catch(TabuleiroException e){
                Console.WriteLine(e.Message);
            }
        }
    }
}
