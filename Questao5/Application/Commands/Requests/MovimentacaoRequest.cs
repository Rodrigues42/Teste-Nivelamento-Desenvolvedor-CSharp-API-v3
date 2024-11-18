namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoRequest
    {
        public string IdentificacaoRequisicao { get; set; }

        public int ContaCorrenteId { get; set; }

        public decimal Valor { get; set; }

        public char TipoMovimento { get; set; }
    }
}
