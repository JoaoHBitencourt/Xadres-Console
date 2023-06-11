using Xadres_Console.Tabuleiro;
using Xadrez_Console.Tabuleiro;

namespace Xadres_Console.Xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro1 tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool Xeque { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro1(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetiraPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = tab.RetiraPeca(destino);
            tab.ColocarPecas(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }

            //Jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetiraPeca(origemT);
                T.IncrementarQtdMovimentos();
                tab.ColocarPecas(T, destinoT);
            }

            //Jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetiraPeca(origemT);
                T.IncrementarQtdMovimentos();
                tab.ColocarPecas(T, destinoT);
            }
            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.RetiraPeca(destino);
            p.DecrementarQtdMovimentos();
            if (pecaCapturada != null)
            {
                tab.ColocarPecas(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPecas(p, origem);

            //Jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetiraPeca(destinoT);
                T.IncrementarQtdMovimentos();
                tab.ColocarPecas(T, origemT);
            }

            //Jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetiraPeca(destinoT);
                T.IncrementarQtdMovimentos();
                tab.ColocarPecas(T, origemT);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutarMovimento(origem, destino);

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroExceptions("Voce não pode se colocar em xeque!");
            }

            if (EstaEmXeque(Adversario(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if (TesteXequemate(Adversario(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.Peca(pos) == null)
            {
                throw new TabuleiroExceptions("Não existe peça na posição de origem escolhida!");
            }

            if (JogadorAtual != tab.Peca(pos).Cor)
            {
                throw new TabuleiroExceptions("A peça de origem escolhida não é sua!");
            }

            if (!tab.Peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroExceptions("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.Peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroExceptions("Posição de destino inválida!");
            }
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor Adversario(Cor cor)
        {
           if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca R = Rei(cor);
            if (R == null)
            {
                throw new TabuleiroExceptions($"Não tem rei da cor {cor} no tabuleiro!");
            }

            foreach (Peca x in PecasEmJogo(Adversario(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequemate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPecas(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));           
            ColocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('b', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('c', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('d', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('e', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('f', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('g', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('h', 2, new Peao(tab, Cor.Branca));

            ColocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('b', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('c', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('d', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('e', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('f', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('g', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('h', 7, new Peao(tab, Cor.Preta));
        }
    }
}
