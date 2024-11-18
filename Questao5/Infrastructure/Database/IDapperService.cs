using Questao5.Application.Commands;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database
{
    public interface IDapperService
    {
        Task<bool> VerificarRequisicaoIdempotente(string identificacaoRequisicao);

        Task<ContaCorrente> ObterContaCorrentePorId(int contaCorrenteId);

        Task<Movimento> ObterMovimentoPorIdMovimento(string movimentacaoId);

        Task<string> PersistirMovimento(ContaCorrente contaCorrente, MovimentacaoCommand request);

        Task PersistirIdempotencia(string identificacaoRequisicao, string request, string result);

        Task<decimal> CalculaSaldo(string contaCorrenteId);
    }
}
