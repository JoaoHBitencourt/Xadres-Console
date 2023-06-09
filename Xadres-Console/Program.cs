using Xadres_Console;
using Xadrez_Console.Tabuleiro;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            tabuleiro1 tab = new tabuleiro1(8, 8);

            Tela.ImprimirTabuleiro(tab);

            Console.WriteLine(tab);
        }
    }
}