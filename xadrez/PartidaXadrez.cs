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

        public bool xeque { get; private set; }

        public Peca vuneralvelEnPassant{get; private set;}

        public PartidaXadrez(Tabuleiro tabuleiro, int turno, Cor jogadorAtual)
        {
            this.tabuleiro = tabuleiro;
            this.turno = turno;
            this.jogadorAtual = jogadorAtual;
            this.terminada = false;
            this.pecas = new HashSet<Peca>();
            this.capturadas = new HashSet<Peca>();
            this.xeque = false;
            vuneralvelEnPassant = null;
        }

        public PartidaXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            xeque = false;
            vuneralvelEnPassant = null;
            colocarPecas();
        }

        /// <summary>
        /// Metodo que diz os adversários
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Cor adversario(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            return Cor.Branca;
        }

        /// <summary>
        /// Metodo que retorna a peça Rei de uma determinada Cor
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Peca rei(Cor cor)
        {
            foreach (Peca item in pecasEmJogo(cor))
            {
                if (item is Rei) return item;
            }
            return null;
        }

        /// <summary>
        /// Verifica se um rei de uma Cor está em xeque 
        /// Com base na matriz de movimentos possiveis de todas as peças
        /// da cor adversária
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);

            if (R == null)
            {
                throw new TabuleiroException("Não existe um rei da cor" + cor + "neste tabuleiro");
            }

            /*
                Para cada peça marcada como adversária, eu verifico se na matriz de movimentos possiveis dela
                existe a posição correspondente ao Rei da Cor em questão
            */
            foreach (Peca item in pecasEmJogo(adversario(cor)))
            {
                bool[,] mat = item.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna]) return true;
            }

            return false;
        }

        /// <summary>
        /// Metodo que dada uma cor, verifica se o Rei se encontra em uma posição de Xeque Mate
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }


            /*
            Laco que verifica se existem alguma peça em jogo da cor 
            que pode realizar um movimento e retirar o rei da posição de xeque
            Obs: Importante entender que ele faz o movimento, obtem o status do xeque
            desfaz o movimento e somente então realiza a verificação que pode gerar um retorno
            */
            foreach (Peca item in pecasEmJogo(cor))
            {
                bool[,] matPos = item.movimentosPossiveis();
                for (int i = 0; i < tabuleiro.linhas; i++)
                {
                    for (int j = 0; j < tabuleiro.colunas; j++)
                    {
                        if (matPos[i, j])
                        {
                            Posicao origem = item.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca capturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, capturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }

                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Metodo que realiza movimentos e adiciona peças ao conjunto de peças capturadas quando necessário
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.removerPeca(origem);
            p.incrementaMovimentos();
            Peca pecaCapturada = tabuleiro.removerPeca(destino);
            tabuleiro.adicionarPeca(p, destino);

            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            //# jogadaEspecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTower = new Posicao(origem.linha, origem.coluna + 3); //+ tres pos é a posição da torre
                Posicao destinoTower = new Posicao(origem.linha, origem.coluna + 1); //+ um pois no roque a torre fica uma posição do rei

                Peca Tower = tabuleiro.removerPeca(origemTower);
                Tower.incrementaMovimentos();
                tabuleiro.adicionarPeca(Tower, destinoTower);
            }

            //# jogadaEspecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTower = new Posicao(origem.linha, origem.coluna - 4); //+ tres pos é a posição da torre
                Posicao destinoTower = new Posicao(origem.linha, origem.coluna - 1); //+ um pois no roque a torre fica uma posição do rei

                Peca Tower = tabuleiro.removerPeca(origemTower);
                Tower.incrementaMovimentos();
                tabuleiro.adicionarPeca(Tower, destinoTower);
            }

            return pecaCapturada;
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
            colocarNovaPeca('a', 1, new Torre(tabuleiro, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(tabuleiro, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(tabuleiro, Cor.Branca));
            colocarNovaPeca('d', 1, new Rainha(tabuleiro, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(tabuleiro, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(tabuleiro, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(tabuleiro, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(tabuleiro, Cor.Branca));
            colocarNovaPeca('a', 2, new Peao(tabuleiro, Cor.Branca, this));
            colocarNovaPeca('b', 2, new Peao(tabuleiro, Cor.Branca, this));
            colocarNovaPeca('c', 2, new Peao(tabuleiro, Cor.Branca, this));
            colocarNovaPeca('d', 2, new Peao(tabuleiro, Cor.Branca, this));
            colocarNovaPeca('e', 2, new Peao(tabuleiro, Cor.Branca, this));
            colocarNovaPeca('f', 2, new Peao(tabuleiro, Cor.Branca, this));
            colocarNovaPeca('g', 2, new Peao(tabuleiro, Cor.Branca, this));
            colocarNovaPeca('h', 2, new Peao(tabuleiro, Cor.Branca, this));

            colocarNovaPeca('a', 8, new Torre(tabuleiro, Cor.Preta));
            colocarNovaPeca('b', 8, new Cavalo(tabuleiro, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(tabuleiro, Cor.Preta));
            colocarNovaPeca('d', 8, new Rainha(tabuleiro, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(tabuleiro, Cor.Preta, this));
            colocarNovaPeca('f', 8, new Bispo(tabuleiro, Cor.Preta));
            colocarNovaPeca('g', 8, new Cavalo(tabuleiro, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(tabuleiro, Cor.Preta));
            colocarNovaPeca('a', 7, new Peao(tabuleiro, Cor.Preta, this));
            colocarNovaPeca('b', 7, new Peao(tabuleiro, Cor.Preta, this));
            colocarNovaPeca('c', 7, new Peao(tabuleiro, Cor.Preta, this));
            colocarNovaPeca('d', 7, new Peao(tabuleiro, Cor.Preta, this));
            colocarNovaPeca('e', 7, new Peao(tabuleiro, Cor.Preta, this));
            colocarNovaPeca('f', 7, new Peao(tabuleiro, Cor.Preta, this));
            colocarNovaPeca('g', 7, new Peao(tabuleiro, Cor.Preta, this));
            colocarNovaPeca('h', 7, new Peao(tabuleiro, Cor.Preta, this));
        }


        // private void colocarPecas()
        // {
        //     colocarNovaPeca('c', 1, new Torre(tabuleiro, Cor.Branca));
        //     colocarNovaPeca('h', 7, new Torre(tabuleiro, Cor.Branca));
        //     colocarNovaPeca('d', 2, new Bispo(tabuleiro, Cor.Branca));
        //     colocarNovaPeca('e', 2, new Bispo(tabuleiro, Cor.Branca));
        //     colocarNovaPeca('e', 1, new Cavalo(tabuleiro, Cor.Branca));
        //     colocarNovaPeca('d', 1, new Rei(tabuleiro, Cor.Branca));

        //     colocarNovaPeca('a', 8, new Rei(tabuleiro, Cor.Preta));
        //     colocarNovaPeca('b', 8, new Torre(tabuleiro, Cor.Preta));
        // }

        /// <summary>
        /// Metodo que executa um movimento dada um posicao de origem e destino
        /// Este metodo tbm valida se o movimento ti levou a uma situacao de xeque
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            Peca pecaMovida = tabuleiro.peca(destino);

            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque");
            }
            if (estaEmXeque(adversario(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (testeXequeMate(adversario(jogadorAtual)))
            {
                terminada = true;
            }

            turno++;
            mudaJoador();

            // #jogadaEspecial = en passant
            if (pecaMovida is Peao && (destino.linha == origem.linha - 2 
                                        || destino.linha == origem.linha + 2))
            {
                vuneralvelEnPassant = pecaMovida;
            }else{
                vuneralvelEnPassant = null;
            }

            // jogadaEspecial en passant
            if(pecaMovida is Peao){
                if(origem.coluna != destino.coluna && pecaCapturada == null){
                    Posicao posP;
                    if(pecaMovida.cor == Cor.Branca){
                        posP = new Posicao(destino.linha +1, destino.coluna);
                    }else{
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tabuleiro.removerPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

        }

        /// <summary>
        /// Metodo que desfaz um movimento dada uma origem, destino e uma possivel peça capturada
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        /// <param name="pecaCapturada"></param>
        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tabuleiro.removerPeca(destino);
            p.decrementaMovimentos();
            if (pecaCapturada != null)
            {
                tabuleiro.adicionarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tabuleiro.adicionarPeca(p, origem);


            //# jogadaEspecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTower = new Posicao(origem.linha, origem.coluna + 3); //+ tres pos é a posição da torre
                Posicao destinoTower = new Posicao(origem.linha, origem.coluna + 1); //+ um pois no roque a torre fica uma posição do rei

                Peca Tower = tabuleiro.removerPeca(origemTower);
                Tower.decrementaMovimentos();
                tabuleiro.adicionarPeca(Tower, destinoTower);
            }

            //# jogadaEspecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTower = new Posicao(origem.linha, origem.coluna - 4); //+ tres pos é a posição da torre
                Posicao destinoTower = new Posicao(origem.linha, origem.coluna - 1); //+ um pois no roque a torre fica uma posição do rei

                Peca Tower = tabuleiro.removerPeca(origemTower);
                Tower.decrementaMovimentos();
                tabuleiro.adicionarPeca(Tower, destinoTower);
            }

            // #jogadaEspecial en passant
            if(p is Peca){
                if(origem.coluna != destino.coluna && pecaCapturada == vuneralvelEnPassant){
                    Peca peao = tabuleiro.removerPeca(destino);
                    Posicao posP;
                    if (p.cor == Cor.Branca){
                        posP = new Posicao(3, destino.coluna);                        
                    }else{
                        posP = new Posicao(4, destino.coluna);
                    }
                    tabuleiro.adicionarPeca(peao, posP);
                }
            }
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
            if (!tabuleiro.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }
    }
}