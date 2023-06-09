namespace Xadrez_Console.Tabuleiro
{
     class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QtdMovimento { get; protected set; }
        public Tabuleiro1 Tab { get; protected set; }

        public Peca(Tabuleiro1 tab, Cor cor)
        {
            this.Posicao = null;
            this.Tab = tab;
            this.Cor = cor;
            this.QtdMovimento = 0;
        }

        public void IncrementarQtdMovimentos()
        {
            QtdMovimento++;
        }

        
    }
}
