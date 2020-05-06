using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using tabuleiro;
namespace xadrez
{
    public class PartidaXadrez
    {
        public Tabuleiro tabuleiro { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }

        public bool terminada { get; private set; }

        private HashSet<Peca> pecas; // guarda todas as peças do jogo
        private HashSet<Peca> capturadas; // guarda as peças capturadas

        public PartidaXadrez(Tabuleiro tabuleiro, int turno, Cor jogadorAtual)
        {
            this.tabuleiro = tabuleiro;
            this.turno = turno;
            this.jogadorAtual = jogadorAtual;
            this.terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
        }

        public PartidaXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        /// <summary>
        /// Metodo que realiza movimentos e adiciona peças ao conjunto de peças capturadas quando necessário
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.removerPeca(origem);
            p.incrementaMovimentos();
            Peca pecaCapturada = tabuleiro.removerPeca(destino);
            tabuleiro.adicionarPeca(p, destino);

            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
        }

        /// <summary>
        /// Método que retorna todas as peças capturadas de uma dada cor
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> retorno = new HashSet<Peca>();

            foreach (Peca item in capturadas)
            {
                if (item.cor == cor) retorno.Add(item);
            }
            return retorno;
        }

        /// <summary>
        /// Metodo que retorna as peças ainda em jogo de uma dada cor
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> pecasEmJogo = new HashSet<Peca>();
            foreach (Peca item in pecas)
            {
                if (item.cor == cor) pecasEmJogo.Add(item);
            }

            pecasEmJogo.ExceptWith(pecasCapturadas(cor));

            return pecasEmJogo;
        }

        /// <summary>
        /// Metodo que adiciona uma peça ao tabuleiro
        /// </summary>
        /// <param name="coluna"></param>
        /// <param name="linha"></param>
        /// <param name="pecaAdd"></param>
        public void colocarNovaPeca(char coluna, int linha, Peca pecaAdd)
        {
            tabuleiro.adicionarPeca(pecaAdd, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(pecaAdd);
        }
        /// <summary>
        /// Metodo privado apeas para iniciar as peças no jogo
        /// </summary>
        private void colocarPecas()
        {
            colocarNovaPeca('c', 1, new Torre(tabuleiro, Cor.Branca));
            colocarNovaPeca('c', 2, new Torre(tabuleiro, Cor.Branca));
            colocarNovaPeca('d', 2, new Bispo(tabuleiro, Cor.Branca));
            colocarNovaPeca('e', 2, new Bispo(tabuleiro, Cor.Branca));
            colocarNovaPeca('e', 1, new Cavalo(tabuleiro, Cor.Branca));
            colocarNovaPeca('d', 1, new Rei(tabuleiro, Cor.Branca));

            colocarNovaPeca('c', 8, new Torre(tabuleiro, Cor.Preta));
            colocarNovaPeca('c', 7, new Torre(tabuleiro, Cor.Preta));
            colocarNovaPeca('d', 7, new Bispo(tabuleiro, Cor.Preta));
            colocarNovaPeca('e', 7, new Bispo(tabuleiro, Cor.Preta));
            colocarNovaPeca('e', 8, new Cavalo(tabuleiro, Cor.Preta));
            colocarNovaPeca('d', 8, new Rei(tabuleiro, Cor.Preta));

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
            if (!tabuleiro.peca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }
    }
}