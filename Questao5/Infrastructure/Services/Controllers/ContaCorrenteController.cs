using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("movimentacao")]
        public async Task<IActionResult> movimentar([FromBody] MovimentacaoRequest movimentacaoRequest)
        {
            var result = await _mediator.Send(new MovimentacaoCommand(movimentacaoRequest));

            if (!result.Sucesso)
            {
                return BadRequest(new { mensagem = result.Mensagem, tipoErro = result.TipoErro });
            }

            return Ok(result);
        }

        [HttpGet("saldo/{contaCorrenteId}")]
        public async Task<IActionResult> getSalto(int contaCorrenteId)
        {
            var result = await _mediator.Send(new GetSaldoCommand(new GetSaldoRequest(contaCorrenteId)));

            if (!result.Sucesso)
            {
                return BadRequest(new { mensagem = result.Mensagem, tipoErro = result.TipoErro });
            }

            return Ok(result);
        }
    }
}
