namespace Questao5.Application.Commands.Requests
{
    public class GetSaldoRequest
    {
        public int ContaCorrenteId { get; set; }

        public GetSaldoRequest(int contaCorrenteId)
        {
            ContaCorrenteId = contaCorrenteId;
        }
    }
}
