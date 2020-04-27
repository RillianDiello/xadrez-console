using System;
using tabuleiro;
namespace xadrez
{
    public class Rainha : Peca
    {
        public Rainha(Tabuleiro tab, Cor cor) : base(tab,cor){}

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
            return "D";
        }
    }
}