namespace Xadrez_Console.Tabuleiro
{
     class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QtdMovimento { get; protected set; }
        public tabuleiro1 Tab { get; protected set; }

        public Peca(tabuleiro1 tab, Cor cor)
        {
            this.Posicao = null;
            this.Tab = tab;
            this.Cor = cor;
            this.QtdMovimento = 0;
        }

        
    }
}
