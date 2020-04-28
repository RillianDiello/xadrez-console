using tabuleiro;
namespace xadrez
{
    public class PosicaoXadrez
    {
        public char coluna {get; set;}
        public int linha {get; set;}

        public const int linhaX =8; 
        public const char colunaX = 'a';

        public PosicaoXadrez(){}
        public PosicaoXadrez(char coluna, int linha)
        {
            this.coluna = coluna;
            this.linha = linha;
        }

        public Posicao toPosicao(){
            return new Posicao(linhaX - linha, coluna - colunaX);
        }

        public override string ToString(){
            return "" + coluna + linha;
        }
    }
}