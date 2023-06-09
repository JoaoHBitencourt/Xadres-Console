﻿using Xadres_Console;
using Xadres_Console.Tabuleiro;
using Xadres_Console.Xadrez;
using Xadrez_Console.Tabuleiro;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                tabuleiro1 tab = new tabuleiro1(8, 8);

                tab.ColocarPecas(new Torre(tab, Cor.Preta), new Posicao(0, 0));
                tab.ColocarPecas(new Torre(tab, Cor.Preta), new Posicao(1, 3));
                tab.ColocarPecas(new Rei(tab, Cor.Preta), new Posicao(0, 9));


                Tela.ImprimirTabuleiro(tab);

                Console.WriteLine(tab);
            }
            catch (TabuleiroExceptions e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}