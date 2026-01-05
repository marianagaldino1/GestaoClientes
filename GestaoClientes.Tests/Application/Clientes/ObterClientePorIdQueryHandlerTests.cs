using GestaoClientes.Application.Clientes.Queries;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.Domain.Entities;
using GestaoClientes.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GestaoClientes.Tests.Application.Clientes
{
    public class ObterClientePorIdQueryHandlerTests
    {
        private readonly Mock<IClienteRepository> _mockRepo;
        private readonly Mock<ILogger<ObterClientePorIdQueryHandler>> _mockLogger;
        private readonly ObterClientePorIdQueryHandler _handler;

        public ObterClientePorIdQueryHandlerTests()
        {
            _mockRepo = new Mock<IClienteRepository>();
            _mockLogger = new Mock<ILogger<ObterClientePorIdQueryHandler>>();
            _handler = new ObterClientePorIdQueryHandler(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task HandleAsync_ClienteExiste_DeveRetornarClienteDto()
        {
            // Arrange
            var cnpj = Cnpj.Criar("01775634000189");
            var cliente = new Cliente("Empresa Teste", cnpj);

            _mockRepo.Setup(r => r.ObterPorIdAsync(cliente.Id))
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

            // Verifica logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Cliente consultado com sucesso")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ClienteNaoExiste_DeveRetornarNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.ObterPorIdAsync(id))
                     .ReturnsAsync((Cliente?)null);

            var query = new ObterClientePorIdQuery(id);

            // Act
            var result = await _handler.HandleAsync(query);

            // Assert
            Assert.Null(result);
            // Verifica logging de warning
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Cliente não encontrado")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Excecao_DeveLogarErroERetornarExcecao()
        {
            // Arrange
            var id = Guid.NewGuid();
            var ex = new Exception("Banco indisponível");

            _mockRepo.Setup(r => r.ObterPorIdAsync(id))
                     .ThrowsAsync(ex);

            var query = new ObterClientePorIdQuery(id);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _handler.HandleAsync(query));

            Assert.Equal(ex, exception);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Erro ao consultar cliente")),
                    ex,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
