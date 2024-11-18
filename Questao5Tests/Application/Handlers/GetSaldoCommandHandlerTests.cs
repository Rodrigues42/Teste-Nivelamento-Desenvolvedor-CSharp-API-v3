using Moq;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database;
using Questao5Tests.Domain.Entities;

namespace Questao5Tests
{
    public class GetSaldoCommandHandlerTests
    {
        private readonly Mock<IDapperService> _dapperServiceMock;
        private readonly GetSaldoCommandHandler _handler;

        public GetSaldoCommandHandlerTests()
        {
            _dapperServiceMock = new Mock<IDapperService>();
            _handler = new GetSaldoCommandHandler(_dapperServiceMock.Object);
        }

        [Fact]
        public void Handle_Success_SaldoResponse()
        {
            var contaCorrente = ContaCorrenteFixture.createContaCorrenteAtiva(123);

            decimal saldo = 1000.99999m;

            var getSaldoCommand = new GetSaldoCommand(new GetSaldoRequest(contaCorrente.Numero));

            _dapperServiceMock.Setup(service => service.ObterContaCorrentePorId(It.IsAny<int>())).ReturnsAsync(contaCorrente);
            _dapperServiceMock.Setup(service => service.CalculaSaldo(contaCorrente.IdContaCorrente)).ReturnsAsync(saldo);

            var response = _handler.Handle(getSaldoCommand, CancellationToken.None).Result;

            Assert.Equal(123, response.NumeroContaCorrente);
            Assert.Equal("Nome", response.Nome);
            Assert.Equal("1000,99", response.Saldo);
        }


        [Fact]
        public void Handle_Error_INVALID_ACCOUNT()
        {
            var getSaldoCommand = new GetSaldoCommand(new GetSaldoRequest(123));

            _dapperServiceMock.Setup(service => service.ObterContaCorrentePorId(It.IsAny<int>())).ReturnsAsync((ContaCorrente) null);

            var response = _handler.Handle(getSaldoCommand, CancellationToken.None).Result;

            Assert.False(response.Sucesso);
            Assert.Equal("Conta corrente não encontrada", response.Mensagem);
            Assert.Equal("INVALID_ACCOUNT", response.TipoErro);
        }

        [Fact]
        public void Handle_Error_INACTIVE_ACCOUNT()
        {
            var contaCorrente = ContaCorrenteFixture.createContaCorrenteInativa(123);

            var getSaldoCommand = new GetSaldoCommand(new GetSaldoRequest(123));

            _dapperServiceMock.Setup(service => service.ObterContaCorrentePorId(It.IsAny<int>())).ReturnsAsync(contaCorrente);

            var response = _handler.Handle(getSaldoCommand, CancellationToken.None).Result;

            Assert.False(response.Sucesso);
            Assert.Equal("Conta corrente inativa", response.Mensagem);
            Assert.Equal("INACTIVE_ACCOUNT", response.TipoErro);
        }
    }
}