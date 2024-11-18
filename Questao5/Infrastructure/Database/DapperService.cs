using Dapper;
using Questao5.Application.Commands;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database
{
    public class DapperService : IDapperService
    {
        private readonly IDbConnection _dbConnection;

        public DapperService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> VerificarRequisicaoIdempotente(string identificacaoRequisicao)
        {
            var resultado = await _dbConnection.QueryFirstOrDefaultAsync<int>(
                "SELECT 1 FROM idempotencia WHERE chave_idempotencia = @IdentificacaoRequisicao",
                new { IdentificacaoRequisicao = identificacaoRequisicao });

            return resultado == 1;
        }

        public async Task<ContaCorrente> ObterContaCorrentePorId(int contaCorrenteId)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE numero = @Id",
                new { Id = contaCorrenteId });
        }

        public async Task<Movimento> ObterMovimentoPorIdMovimento(string movimentacaoId)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<Movimento>(
                "SELECT * FROM movimento WHERE idmovimento = @Id",
                new { Id = movimentacaoId });
        }

        public async Task<string> PersistirMovimento(ContaCorrente contaCorrente, MovimentacaoCommand request)
        {
            var movimentoInsert = await _dbConnection.ExecuteAsync(
                @"
                    INSERT INTO movimento (idmovimento, idcontacorrente, valor, tipomovimento, datamovimento)
                    VALUES (@MovimentacaoId, @ContaCorrenteId, @Valor, @TipoMovimento, strftime('%d/%m/%Y', 'now'));
                ",
                new
                {
                    MovimentacaoId = request.IdentificacaoRequisicao,
                    ContaCorrenteId = contaCorrente.IdContaCorrente,
                    request.Valor,
                    request.TipoMovimento
                });

            var movimento = await ObterMovimentoPorIdMovimento(request.IdentificacaoRequisicao);

            return movimento.IdMovimento;

        }

        public async Task PersistirIdempotencia(string identificacaoRequisicao, string requisicao, string resultado)
        {
                await _dbConnection.ExecuteAsync(
                @"
                    INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                    VALUES (@Chave_idempotencia, @Requisicao, @Resultado);
                ",
                new
                {
                    Chave_idempotencia = identificacaoRequisicao,
                    Requisicao = requisicao,
                    Resultado = resultado
                });
        }

        public async Task<decimal> CalculaSaldo(string contaCorrenteId)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<decimal>(
                @"SELECT 
                    SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END) - 
    	            SUM(CASE WHEN tipomovimento = 'D' THEN Valor ELSE 0 END)
                FROM movimento
                WHERE IdContaCorrente = @ContaCorrenteId",
                new { ContaCorrenteId = contaCorrenteId });
        }
    }
}
