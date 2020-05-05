using System;
using tabuleiro;
namespace xadrez
{
    public class PartidaXadrez
    {
        public Tabuleiro tabuleiro { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }

        public bool terminada { get; private set; }

        public PartidaXadrez(Tabuleiro tabuleiro, int turno, Cor jogadorAtual)
        {
            this.tabuleiro = tabuleiro;
            this.turno = turno;
            this.jogadorAtual = jogadorAtual;
            this.terminada = false;
        }

        public PartidaXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            colocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.removerPeca(origem);
            p.incrementaMovimentos();
            Peca capturada = tabuleiro.removerPeca(destino);

            tabuleiro.adicionarPeca(p, destino);
        }



        private void colocarPecas()
        {


            tabuleiro.adicionarPeca(new Torre(tabuleiro, Cor.Branca), new PosicaoXadrez('c', 1).toPosicao());
            tabuleiro.adicionarPeca(new Torre(tabuleiro, Cor.Branca), new PosicaoXadrez('c', 2).toPosicao());
            tabuleiro.adicionarPeca(new Bispo(tabuleiro, Cor.Branca), new PosicaoXadrez('d', 2).toPosicao());
            tabuleiro.adicionarPeca(new Bispo(tabuleiro, Cor.Branca), new PosicaoXadrez('e', 2).toPosicao());
            tabuleiro.adicionarPeca(new Cavalo(tabuleiro, Cor.Branca), new PosicaoXadrez('e', 1).toPosicao());
            tabuleiro.adicionarPeca(new Rei(tabuleiro, Cor.Branca), new PosicaoXadrez('d', 1).toPosicao());

            tabuleiro.adicionarPeca(new Torre(tabuleiro, Cor.Preta), new PosicaoXadrez('c', 8).toPosicao());
            tabuleiro.adicionarPeca(new Torre(tabuleiro, Cor.Preta), new PosicaoXadrez('c', 7).toPosicao());
            tabuleiro.adicionarPeca(new Bispo(tabuleiro, Cor.Preta), new PosicaoXadrez('d', 7).toPosicao());
            tabuleiro.adicionarPeca(new Bispo(tabuleiro, Cor.Preta), new PosicaoXadrez('e', 7).toPosicao());
            tabuleiro.adicionarPeca(new Cavalo(tabuleiro, Cor.Preta), new PosicaoXadrez('e', 8).toPosicao());
            tabuleiro.adicionarPeca(new Rei(tabuleiro, Cor.Preta), new PosicaoXadrez('d', 8).toPosicao());

        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            turno++;
            mudaJoador();

        }

        private void mudaJoador()
        {
            if (jogadorAtual == Cor.Branca)
                jogadorAtual = Cor.Preta;
            else jogadorAtual = Cor.Branca;
        }

        /// <summary>
        /// Método que valida se uma determinada posição origem de movimento
        /// 1) Contem uma peça
        /// 2) A peça pertence ao jogador do Turno
        /// 3) A peça não está com movimentos bloqueados
        /// </summary>
        /// <param name="origem"></param>
        public void validarPosicaoOrigem(Posicao origem)
        {
            if (tabuleiro.peca(origem) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida");
            }
            if (jogadorAtual != tabuleiro.peca(origem).cor)
            {
                throw new TabuleiroException("A peça de origem não pertence a este jogador");
            }
            if (!tabuleiro.peca(origem).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não existem movimentos possiveis para a peça selecionada");
            }
        }

        /// <summary>
        /// Metodo que valida se uma determina posição (destino) é acessivel a uma peça, mediante sua atual (origem)
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public void validarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if(!tabuleiro.peca(origem).podeMoverPara(destino)){
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }
    }
}