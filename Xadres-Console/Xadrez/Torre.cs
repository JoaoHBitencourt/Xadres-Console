using Xadrez_Console.Tabuleiro;

namespace Xadres_Console.Xadrez
{
    class Torre : Peca
    {
        public Torre(tabuleiro1 tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "T";
        }
    }
}
