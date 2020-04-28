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
            Tabuleiro tab = new Tabuleiro(8,8);

            // tab.adicionarPeca(new Torre(tab, Cor.Preta), new Posicao(0,0));
            // tab.adicionarPeca(new Rei(tab, Cor.Preta), new Posicao(1,2));
            // tab.adicionarPeca(new Bispo(tab, Cor.Preta), new Posicao(0,2));

            PosicaoXadrez posicaoXadrez = new PosicaoXadrez('c', 7);
            Console.WriteLine(posicaoXadrez.toPosicao());
            // Tela.imprimirTabuleiro(tab);
            }catch(TabuleiroException e){
                Console.WriteLine(e.Message);
            }
        }
    }
}
