using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands
{
    public class MovimentacaoCommand : IRequest<MovimentacaoResponse>
    {
        public string IdentificacaoRequisicao { get; set; }

        public int ContaCorrenteId { get; set; }

        public decimal Valor { get; set; }

        public char TipoMovimento { get; set; }

        public MovimentacaoCommand(MovimentacaoRequest request)
        {
            IdentificacaoRequisicao = request.IdentificacaoRequisicao;
            ContaCorrenteId = request.ContaCorrenteId;
            Valor = request.Valor;
            TipoMovimento = request.TipoMovimento;
        }
    }
}
