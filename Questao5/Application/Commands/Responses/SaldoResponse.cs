namespace Questao5.Application.Commands.Responses
{
    public class SaldoResponse : BaseResponse
    {
        public int NumeroContaCorrente { get; set; }

        public string Nome { get; set; }

        public string Date { get; set; }

        public string Saldo { get; set; }
    }
}
