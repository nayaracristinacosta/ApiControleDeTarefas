using ApiControleDeTarefas.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiControleDeTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiControleDeTarefas.Domain.Models;
namespace ApiControleDeTarefas.Controllers
{
    [Authorize]
    [ApiController]
    public class TarefaExtraController : ControllerBase
    {
        private readonly TarefaExtraService _service;
        public TarefaExtraController(TarefaExtraService service)
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
        [HttpGet("TarefaExtra")]
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
        [HttpGet("TarefaExtra/{tarefaExtraiId}")]
        public IActionResult ObterPorId([FromRoute] int tarefaExtraiId)
        {
            return StatusCode(200, _service.Obter(tarefaExtraiId));
        }

        /// <summary>
        /// Através dessa rota você será capaz de cadastrar um cargo - 
        /// Campos obrigatórios: descricao
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "2")]
        [HttpPost("TarefaExtra")]
        public IActionResult Inserir([FromBody] TarefaExtra model)
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
        [Authorize(Roles = "2")]
        [HttpDelete("TarefaExtra/{tarefaExtraiId}")]
        public IActionResult Deletar([FromRoute] int tarefaExtraiId)
        {
            _service.Deletar(tarefaExtraiId);
            return StatusCode(200);
        }

        /// <summary>
        /// Através dessa rota você será capaz de atualizar um cargo - 
        /// Campos obrigatórios: cargoId, descricao
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = "2")]
        [HttpPut("TarefaExtra")]
        public IActionResult Atualizar([FromBody] TarefaExtra model)
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
