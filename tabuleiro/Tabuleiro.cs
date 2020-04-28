namespace tabuleiro
{
    public class Tabuleiro
    {

        
        public int linhas { get; set; }
        public int colunas { get; set; }

        private Peca[,] pecas;

        public Tabuleiro(){}
        public Tabuleiro(int linhas, int colunas)
        {
            this.linhas = linhas;
            this.colunas = colunas;
            pecas = new Peca[linhas, colunas];
        }

        public Peca peca(int linha, int coluna){
            return pecas[linha,coluna];
        }

        public Peca peca(Posicao pos){
            return pecas[pos.linha, pos.coluna];
        }

        public void adicionarPeca(Peca p, Posicao pos){
            if(existePeca(pos)){
                throw new TabuleiroException("Já existe uma peça nesta posição");
            }
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }

        public Peca removerPeca(Posicao posicao){
            if(peca(posicao) == null){
                return null;
            }
            Peca aux = peca(posicao);
            aux.posicao = null;
            pecas[posicao.linha, posicao.coluna] = null;
            return aux;
        }

        public bool existePeca(Posicao pos){
            validarPosicao(pos);
            return peca(pos) != null;
        }

        public bool posicaoValida(Posicao posicao){
            if(posicao.linha < 0 || posicao.linha > linhas || posicao.coluna < 0 || posicao.coluna > colunas){
                return false;
            }
            return true;
        }

        public void validarPosicao(Posicao posicao){
            if(!posicaoValida(posicao)){
                throw new TabuleiroException("Posição inválida");
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}