using Xadrez_Console.Tabuleiro;

namespace Xadres_Console.Xadrez
{
    class Rei : Peca
    {
        public Rei(tabuleiro1 tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
