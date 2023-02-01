using ApiControleDeTarefas.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiControleDeTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;

namespace ApiControleDeTarefas.Controllers
{
    [Authorize]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _service;
        public TarefaController(TarefaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Através dessa rota você será capaz de listar um cargo - 
        /// Campos obrigatórios: descricao
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1,2,3")]
        [HttpGet("Tarefa")]
        public IActionResult Listar([FromQuery] string? descricao)
        {
            return StatusCode(200, _service.Listar(descricao));
        }

        /// <summary>
        /// Através dessa rota você será capaz de listar um cargo -
        /// Campos obrigatórios: cargoId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1,2,3")]
        [HttpGet("Tarefa/{tarefaiId}")]
        public IActionResult ObterPorId([FromRoute] int tarefaiId)
        {
            return StatusCode(200, _service.Obter(tarefaiId));
        }

        /// <summary>
        /// Através dessa rota você será capaz de cadastrar um cargo - 
        /// Campos obrigatórios: descricao
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1,2")]
        [HttpPost("Tarefa")]
        public IActionResult Inserir([FromBody] TarefaRequest model)
        {
            try
            {
                _service.Inserir(model);
                return StatusCode(201);
            }
            catch (ValidacaoException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
        /// <summary>
        /// Através dessa rota você será capaz de deletar um cargo - 
        /// Campos obrigatórios: cargoId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1,2")]
        [HttpDelete("Tarefa/{tarefaiId}")]
        public IActionResult Deletar([FromRoute] int tarefaiId)
        {
            _service.Deletar(tarefaiId);
            return StatusCode(200);
        }

        /// <summary>
        /// Através dessa rota você será capaz de atualizar um cargo - 
        /// Campos obrigatórios: cargoId, descricao
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "1,2")]
        [HttpPut("Tarefa")]
        public IActionResult Atualizar([FromBody] Tarefa model)
        {
            try
            {
                _service.Atualizar(model);
                return StatusCode(201);
            }
            catch (ValidacaoException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
