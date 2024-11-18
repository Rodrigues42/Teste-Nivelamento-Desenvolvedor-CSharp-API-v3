using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands
{
    public class GetSaldoCommand : IRequest<SaldoResponse>
    {
        public int ContaCorrenteId { get; set; }

        public GetSaldoCommand(GetSaldoRequest request)
        { 
            ContaCorrenteId = request.ContaCorrenteId;
        }
    }
}
