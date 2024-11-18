using Moq;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database;
using Questao5Tests.Application.Commands;
using Questao5Tests.Domain.Entities;

namespace Questao5Tests.Application.Handlers
{
    public class MovimentacaoCommandHandlerTests
    {
        private readonly Mock<IDapperService> _dapperServiceMock;
        private readonly MovimentacaoCommandHandler _handler;

        public MovimentacaoCommandHandlerTests()
        {
            _dapperServiceMock = new Mock<IDapperService>();
            _handler = new MovimentacaoCommandHandler(_dapperServiceMock.Object);
        }

        [Fact]
        public void Handle_Success_MovimentacaoResponse()
        {
            var contaCorrente = ContaCorrenteFixture.createContaCorrenteAtiva(123);
            var movimentacaoCommand = MovimentacaoCommandFixture.createMovimentacaoCommandGeneric(contaCorrente.Numero, 1000.00m, 'C');

            _dapperServiceMock.Setup(service => service.VerificarRequisicaoIdempotente(It.IsAny<string>())).ReturnsAsync(false);
            _dapperServiceMock.Setup(service => service.ObterContaCorrentePorId(It.IsAny<int>())).ReturnsAsync(contaCorrente);
            _dapperServiceMock.Setup(service => service.PersistirMovimento(contaCorrente, movimentacaoCommand)).ReturnsAsync(movimentacaoCommand.IdentificacaoRequisicao);
            _dapperServiceMock.Setup(service => service.PersistirIdempotencia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var response = _handler.Handle(movimentacaoCommand, CancellationToken.None).Result;

            Assert.True(response.Sucesso);
            Assert.Equal(movimentacaoCommand.IdentificacaoRequisicao, response.IdMovimento);
        }

        [Fact]
        public void Handle_Error_IDEMPOTENCIA()
        {
            var contaCorrente = ContaCorrenteFixture.createContaCorrenteAtiva(123);
            var movimentacaoCommand = MovimentacaoCommandFixture.createMovimentacaoCommandGeneric(contaCorrente.Numero, 1000.00m, 'C');

            _dapperServiceMock.Setup(service => service.VerificarRequisicaoIdempotente(It.IsAny<string>())).ReturnsAsync(true);

            var response = _handler.Handle(movimentacaoCommand, CancellationToken.None).Result;

            Assert.False(response.Sucesso);
            Assert.Equal("Requisição já processada.", response.Mensagem);
            Assert.Equal("IDEMPOTENCIA", response.TipoErro);
        }

        [Fact]
        public void Handle_Error_INVALID_ACCOUNT()
        {
            var contaCorrente = ContaCorrenteFixture.createContaCorrenteAtiva(123);
            var movimentacaoCommand = MovimentacaoCommandFixture.createMovimentacaoCommandGeneric(contaCorrente.Numero, 1000.00m, 'C');

            _dapperServiceMock.Setup(service => service.VerificarRequisicaoIdempotente(It.IsAny<string>())).ReturnsAsync(false);
            _dapperServiceMock.Setup(service => service.ObterContaCorrentePorId(It.IsAny<int>())).ReturnsAsync((ContaCorrente) null);

            var response = _handler.Handle(movimentacaoCommand, CancellationToken.None).Result;

            Assert.False(response.Sucesso);
            Assert.Equal("Conta corrente não cadastrada.", response.Mensagem);
            Assert.Equal("INVALID_ACCOUNT", response.TipoErro);
        }

        [Fact]
        public void Handle_Error_INACTIVE_ACCOUNT()
        {
            var contaCorrente = ContaCorrenteFixture.createContaCorrenteInativa(123);
            var movimentacaoCommand = MovimentacaoCommandFixture.createMovimentacaoCommandGeneric(contaCorrente.Numero, 1000.00m, 'C');

            _dapperServiceMock.Setup(service => service.VerificarRequisicaoIdempotente(It.IsAny<string>())).ReturnsAsync(false);
            _dapperServiceMock.Setup(service => service.ObterContaCorrentePorId(It.IsAny<int>())).ReturnsAsync(contaCorrente);

            var response = _handler.Handle(movimentacaoCommand, CancellationToken.None).Result;

            Assert.False(response.Sucesso);
            Assert.Equal("Conta corrente inativa.", response.Mensagem);
            Assert.Equal("INACTIVE_ACCOUNT", response.TipoErro);
        }

        [Fact]
        public void Handle_Error_INVALID_VALUE()
        {
            var contaCorrente = ContaCorrenteFixture.createContaCorrenteAtiva(123);
            var movimentacaoCommand = MovimentacaoCommandFixture.createMovimentacaoCommandGeneric(contaCorrente.Numero, -1000.00m, 'C');

            _dapperServiceMock.Setup(service => service.VerificarRequisicaoIdempotente(It.IsAny<string>())).ReturnsAsync(false);
            _dapperServiceMock.Setup(service => service.ObterContaCorrentePorId(It.IsAny<int>())).ReturnsAsync(contaCorrente);

            var response = _handler.Handle(movimentacaoCommand, CancellationToken.None).Result;

            Assert.False(response.Sucesso);
            Assert.Equal("Valor inválido.", response.Mensagem);
            Assert.Equal("INVALID_VALUE", response.TipoErro);
        }

        [Fact]
        public void Handle_Error_INVALID_TYPE()
        {
            var contaCorrente = ContaCorrenteFixture.createContaCorrenteAtiva(123);
            var movimentacaoCommand = MovimentacaoCommandFixture.createMovimentacaoCommandGeneric(contaCorrente.Numero, 1000.00m, 'S');

            _dapperServiceMock.Setup(service => service.VerificarRequisicaoIdempotente(It.IsAny<string>())).ReturnsAsync(false);
            _dapperServiceMock.Setup(service => service.ObterContaCorrentePorId(It.IsAny<int>())).ReturnsAsync(contaCorrente);

            var response = _handler.Handle(movimentacaoCommand, CancellationToken.None).Result;

            Assert.False(response.Sucesso);
            Assert.Equal("Tipo de movimento inválido.", response.Mensagem);
            Assert.Equal("INVALID_TYPE", response.TipoErro);
        }
    }
}
