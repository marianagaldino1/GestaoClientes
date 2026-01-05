using GestaoClientes.Application.Clientes.Queries;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.Domain.Entities;
using GestaoClientes.Domain.ValueObjects;
using Moq;
using Xunit;

namespace GestaoClientes.Tests.Application.Clientes
{
    public class ObterClientePorIdQueryHandlerTests
    {
        private readonly Mock<IClienteRepository> _mockRepo;
        private readonly ObterClientePorIdQueryHandler _handler;

        public ObterClientePorIdQueryHandlerTests()
        {
            _mockRepo = new Mock<IClienteRepository>();
            _handler = new ObterClientePorIdQueryHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task HandleAsync_ClienteExiste_DeveRetornarClienteDto()
        {
            // Arrange
            var cnpj = Cnpj.Criar("01775634000189");
            var cliente = new Cliente("Empresa Teste", cnpj);

            _mockRepo
                .Setup(r => r.ObterPorIdAsync(cliente.Id))
                .ReturnsAsync(cliente);

            var query = new ObterClientePorIdQuery(cliente.Id);

            // Act
            var result = await _handler.HandleAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cliente.Id, result!.Id);
            Assert.Equal(cliente.Nome, result.Nome);
            Assert.Equal(cliente.Cnpj.Valor, result.Cnpj);
            Assert.Equal(cliente.Ativo, result.Ativo);
        }

        [Fact]
        public async Task HandleAsync_ClienteNaoExiste_DeveRetornarNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepo
                .Setup(r => r.ObterPorIdAsync(id))
                .ReturnsAsync((Cliente?)null);

            var query = new ObterClientePorIdQuery(id);

            // Act
            var result = await _handler.HandleAsync(query);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task HandleAsync_RepositorioLancaExcecao_DevePropagarExcecao()
        {
            // Arrange
            var id = Guid.NewGuid();
            var ex = new Exception("Banco indisponível");

            _mockRepo
                .Setup(r => r.ObterPorIdAsync(id))
                .ThrowsAsync(ex);

            var query = new ObterClientePorIdQuery(id);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _handler.HandleAsync(query));

            Assert.Equal(ex, exception);
        }
    }
}
