using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;

namespace Questao5Tests.Application.Commands
{
    public class MovimentacaoCommandFixture
    {
        public static MovimentacaoCommand createMovimentacaoCommandGeneric(int contaCorrenteId, decimal valor, char tipoMovimento)
        {
            return new MovimentacaoCommand(new MovimentacaoRequest()
            {
                IdentificacaoRequisicao = "Teste_1",
                ContaCorrenteId = contaCorrenteId,
                Valor = valor,
                TipoMovimento = tipoMovimento
            });
        }
    }
}
