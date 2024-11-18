using Questao5.Application.Commands.Responses;
using Questao5.Application.Commands;
using Questao5.Infrastructure.Database;
using MediatR;

namespace Questao5.Application.Handlers
{
    public class GetSaldoCommandHandler : IRequestHandler<GetSaldoCommand, SaldoResponse>
    {
        private readonly IDapperService _dapperService;

        public GetSaldoCommandHandler(IDapperService dapperService)
        {
            _dapperService = dapperService;
        }

        public async Task<SaldoResponse> Handle(GetSaldoCommand request, CancellationToken cancellationToken)
        {

            var contaCorrente = await _dapperService.ObterContaCorrentePorId(request.ContaCorrenteId);

            if (contaCorrente == null)
            {
                return new SaldoResponse
                {
                    Sucesso = false,
                    Mensagem = "Conta corrente não encontrada",
                    TipoErro = "INVALID_ACCOUNT"
                };
            }

            if (!contaCorrente.Ativo)
            {
                return new SaldoResponse
                {
                    Sucesso = false,
                    Mensagem = "Conta corrente inativa",
                    TipoErro = "INACTIVE_ACCOUNT"
                };
            }

            var saldo = await _dapperService.CalculaSaldo(contaCorrente.IdContaCorrente);

            // Retorna a resposta com os dados da conta e o saldo
            var response = new SaldoResponse
            {
                NumeroContaCorrente = contaCorrente.Numero,
                Nome = contaCorrente.Nome,
                Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                Saldo = (Math.Truncate(saldo * 100) / 100).ToString("F2")
            };

            return response;
        }
    }
}
