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

        public void adicionarPeca(Peca p, Posicao pos){
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
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