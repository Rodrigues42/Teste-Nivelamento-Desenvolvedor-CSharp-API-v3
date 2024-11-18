using MediatR;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Database;
using System.Text.Json;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoCommandHandler : IRequestHandler<MovimentacaoCommand, MovimentacaoResponse>
    {
        private readonly IDapperService _dapperService;

        public MovimentacaoCommandHandler(IDapperService dapperService)
        {
            _dapperService = dapperService;
        }

        public async Task<MovimentacaoResponse> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
        {

            if (await _dapperService.VerificarRequisicaoIdempotente(request.IdentificacaoRequisicao))
            {
                return new MovimentacaoResponse
                {
                    Sucesso = false,
                    Mensagem = "Requisição já processada.",
                    TipoErro = "IDEMPOTENCIA"
                };
            }

            var contaCorrente = await _dapperService.ObterContaCorrentePorId(request.ContaCorrenteId);
            if (contaCorrente == null)
            {
                return new MovimentacaoResponse
                {
                    Sucesso = false,
                    Mensagem = "Conta corrente não cadastrada.",
                    TipoErro = "INVALID_ACCOUNT"
                };
            }

            if (!contaCorrente.Ativo)
            {
                return new MovimentacaoResponse
                {
                    Sucesso = false,
                    Mensagem = "Conta corrente inativa.",
                    TipoErro = "INACTIVE_ACCOUNT"
                };
            }

            if (request.Valor <= 0)
            {
                return new MovimentacaoResponse
                {
                    Sucesso = false,
                    Mensagem = "Valor inválido.",
                    TipoErro = "INVALID_VALUE"
                };
            }

            if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
            {
                return new MovimentacaoResponse
                {
                    Sucesso = false,
                    Mensagem = "Tipo de movimento inválido.",
                    TipoErro = "INVALID_TYPE"
                };
            }

            var movimentoId = await _dapperService.PersistirMovimento(contaCorrente, request);
            var response = new MovimentacaoResponse
            {
                Sucesso = true,
                IdMovimento = movimentoId
            };

            await _dapperService.PersistirIdempotencia(request.IdentificacaoRequisicao, JsonSerializer.Serialize(request), JsonSerializer.Serialize(response));

            return response;
        }
    }
}
