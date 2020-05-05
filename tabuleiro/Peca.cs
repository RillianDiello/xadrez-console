
namespace tabuleiro
{
    public abstract class Peca
    {

        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int qteMovimentos { get; protected set; }

        public Tabuleiro tab { get; protected set; }

        public Peca()
        {

        }

        public Peca(Tabuleiro tab, Cor cor)
        {
            this.posicao = null;
            this.tab = tab;
            this.cor = cor;
            this.qteMovimentos = 0;
        }

        public void incrementaMovimentos()
        {
            qteMovimentos++;
        }

        public abstract bool[,] movimentosPossiveis();

        /// <summary>
        /// Metodo que verifica se eu posso mover a peça em questão
        /// </summary>
        /// <returns></returns>
        public bool existeMovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();

            for (int i = 0; i < tab.linhas; i++)
            {
                for (int j = 0; j < tab.colunas; j++)
                {
                    if (mat[i, j]) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Metodo que verifica se um determinado destino está presente na matriz de uma peça
        /// </summary>
        /// <param name="destino"></param>
        /// <returns></returns>
        public bool podeMoverPara(Posicao destino)
        {
            return movimentosPossiveis()[destino.linha, destino.coluna];
        }
        
    }
}