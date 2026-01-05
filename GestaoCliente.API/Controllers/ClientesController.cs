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

        public ClientesController(
            CriarClienteCommandHandler criaHandler,
            ObterClientePorIdQueryHandler consultaHandler)
        {
            _criaHandler = criaHandler;
            _consultaHandler = consultaHandler;
            
        }

        [HttpPost]
        public async Task<IActionResult> Post(CriarClienteCommand command)
        {
            try
            {
                var id = await _criaHandler.HandleAsync(command);
                
                return CreatedAtAction(nameof(Get), new { id }, new { Id = id });
            }
            catch (DomainException ex)
            {
                
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
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
                    return NotFound();
                }

               
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { erro = "Ocorreu um erro interno." });
            }
        }
    }
}
