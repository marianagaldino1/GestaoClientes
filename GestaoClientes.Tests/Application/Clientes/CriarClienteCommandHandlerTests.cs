using GestaoClientes.Application.Clientes.Commands;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.Domain.Entities;
using GestaoClientes.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Tests.Application.Clientes
{
    public class CriarClienteCommandHandlerTests
    {
        private readonly Mock<IClienteRepository> _mockRepo;
        private readonly CriarClienteCommandHandler _handler;

        public CriarClienteCommandHandlerTests()
        {
            _mockRepo = new Mock<IClienteRepository>();
            _handler = new CriarClienteCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task HandleAsync_CriarClienteValido_DeveRetornarId()
        {
            var command = new CriarClienteCommand("Empresa Teste", "01775634000189");

            _mockRepo
                .Setup(r => r.ExisteCnpjAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockRepo
                .Setup(r => r.AdicionarAsync(It.IsAny<Cliente>()))
                .Returns(Task.CompletedTask);

            var id = await _handler.HandleAsync(command);

            Assert.NotEqual(Guid.Empty, id);

            _mockRepo.Verify(
                r => r.ExisteCnpjAsync("01775634000189"),
                Times.Once);

            _mockRepo.Verify(
                r => r.AdicionarAsync(It.IsAny<Cliente>()),
                Times.Once);
        }

        [Fact]
        public async Task HandleAsync_CnpjDuplicado_DeveLancarDomainException()
        {
            var command = new CriarClienteCommand("Empresa Teste", "01775634000189");

            _mockRepo
                .Setup(r => r.ExisteCnpjAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<DomainException>(
                () => _handler.HandleAsync(command));

            Assert.Contains("Já existe cliente", ex.Message);

            _mockRepo.Verify(
                r => r.AdicionarAsync(It.IsAny<Cliente>()),
                Times.Never);
        }

        [Fact]
        public async Task HandleAsync_NomeInvalido_DeveLancarDomainException()
        {
            var command = new CriarClienteCommand("", "01775634000189");

            await Assert.ThrowsAsync<DomainException>(
                () => _handler.HandleAsync(command));

            _mockRepo.Verify(
                r => r.AdicionarAsync(It.IsAny<Cliente>()),
                Times.Never);
        }

        [Fact]
        public async Task HandleAsync_CnpjInvalido_DeveLancarDomainException()
        {
            var command = new CriarClienteCommand("Empresa Teste", "00000000000000");

            await Assert.ThrowsAsync<DomainException>(
                () => _handler.HandleAsync(command));

            _mockRepo.Verify(
                r => r.AdicionarAsync(It.IsAny<Cliente>()),
                Times.Never);
        }
    }

}
