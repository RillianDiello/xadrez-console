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
                

                while(!partida.terminada){
                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tabuleiro);
                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.lerPosicaoXadrez().toPosicao();

                    bool[,] posicoesPossiveis = partida.tabuleiro.peca(origem).movimentosPossiveis();

                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tabuleiro, posicoesPossiveis);
                    
                    Console.WriteLine();
                    Console.Write("Destino: ");
                    Posicao destino = Tela.lerPosicaoXadrez().toPosicao();

                    partida.ExecutaMovimento(origem,destino);

                    Console.WriteLine();
                }

            // PosicaoXadrez posicaoXadrez = new PosicaoXadrez('c', 7);
            // Console.WriteLine(posicaoXadrez.toPosicao());
            Tela.imprimirTabuleiro(partida.tabuleiro);
            }catch(TabuleiroException e){
                Console.WriteLine(e.Message);
            }
        }
    }
}
