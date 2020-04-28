using System;
using tabuleiro;
namespace xadrez
{
    public class PartidaXadrez
    {
        public Tabuleiro tabuleiro{get;private set;}
        private int turno;
        private Cor jogadorAtual;

        public PartidaXadrez(Tabuleiro tabuleiro, int turno, Cor jogadorAtual)
        {
            this.tabuleiro = tabuleiro;
            this.turno = turno;
            this.jogadorAtual = jogadorAtual;
            
        }

        public PartidaXadrez(){
            tabuleiro = new Tabuleiro(8,8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            colocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino){
            Peca p = tabuleiro.removerPeca(origem);
            p.incrementaMovimentos();
            Peca capturada = tabuleiro.removerPeca(destino);

            tabuleiro.adicionarPeca(p, destino);
        }

       

        private void colocarPecas(){

            
            tabuleiro.adicionarPeca(new Torre(tabuleiro, Cor.Branca), new PosicaoXadrez('c',1).toPosicao());
            tabuleiro.adicionarPeca(new Torre(tabuleiro, Cor.Branca), new PosicaoXadrez('c',2).toPosicao());
            tabuleiro.adicionarPeca(new Bispo(tabuleiro, Cor.Branca), new PosicaoXadrez('d',2).toPosicao());
            tabuleiro.adicionarPeca(new Bispo(tabuleiro, Cor.Branca), new PosicaoXadrez('e',2).toPosicao());
            tabuleiro.adicionarPeca(new Cavalo(tabuleiro, Cor.Branca), new PosicaoXadrez('e',1).toPosicao());
            tabuleiro.adicionarPeca(new Rei(tabuleiro, Cor.Branca), new PosicaoXadrez('d',1).toPosicao());

            tabuleiro.adicionarPeca(new Torre(tabuleiro, Cor.Preta), new PosicaoXadrez('c',8).toPosicao());
            tabuleiro.adicionarPeca(new Torre(tabuleiro, Cor.Preta), new PosicaoXadrez('c',7).toPosicao());
            tabuleiro.adicionarPeca(new Bispo(tabuleiro, Cor.Preta), new PosicaoXadrez('d',7).toPosicao());
            tabuleiro.adicionarPeca(new Bispo(tabuleiro, Cor.Preta), new PosicaoXadrez('e',7).toPosicao());
            tabuleiro.adicionarPeca(new Cavalo(tabuleiro, Cor.Preta), new PosicaoXadrez('e',8).toPosicao());
            tabuleiro.adicionarPeca(new Rei(tabuleiro, Cor.Preta), new PosicaoXadrez('d',8).toPosicao());
            
        }
    }
}