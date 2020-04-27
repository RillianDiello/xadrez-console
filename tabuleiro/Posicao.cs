namespace tabuleiro
{
    public class Posicao
    {
        public Posicao(int linha, int coluna)
        {
            this.linha = linha;
            this.coluna = coluna;

        }

        public Posicao(){

        }

       
        public int linha { get; set; }
        public int coluna { get; set; }

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
            return linha + ", " + coluna;
        }
    }
}