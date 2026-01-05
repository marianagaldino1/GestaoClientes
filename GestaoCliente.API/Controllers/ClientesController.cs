using GestaoClientes.Application.Clientes.Commands;
using GestaoClientes.Application.Clientes.Queries;
using GestaoClientes.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GestaoClientes.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly CriarClienteCommandHandler _criaHandler;
        private readonly ObterClientePorIdQueryHandler _consultaHandler;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(
            CriarClienteCommandHandler criaHandler,
            ObterClientePorIdQueryHandler consultaHandler,
            ILogger<ClientesController> logger)
        {
            _criaHandler = criaHandler;
            _consultaHandler = consultaHandler;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CriarClienteCommand command)
        {
            try
            {
                var id = await _criaHandler.HandleAsync(command);
                _logger.LogInformation("Cliente criado com sucesso. Id: {Id}", id);
                return CreatedAtAction(nameof(Get), new { id }, new { Id = id });
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Falha ao criar cliente. Dados inválidos: {@Command}", command);
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao criar cliente. Dados: {@Command}", command);
                return StatusCode(500, new { erro = "Ocorreu um erro interno." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var cliente = await _consultaHandler.HandleAsync(new ObterClientePorIdQuery(id));

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado. Id: {Id}", id);
                    return NotFound();
                }

                _logger.LogInformation("Cliente consultado com sucesso. Id: {Id}", id);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao consultar cliente. Id: {Id}", id);
                return StatusCode(500, new { erro = "Ocorreu um erro interno." });
            }
        }
    }
}
