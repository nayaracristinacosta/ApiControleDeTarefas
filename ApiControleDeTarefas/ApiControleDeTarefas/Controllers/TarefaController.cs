using ApiControleDeTarefas.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiControleDeTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;
using System.Security.Claims;
using ApiControleDeTarefas.Domain.Utils;

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
        /// Através dessa rota você será capaz de listar tarefas - 
        /// Campos obrigatórios: O campo descrição é opcional 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Geral)]
        [HttpGet("Tarefa")]
        public IActionResult Listar([FromQuery] string? descricao)
        {
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var login = identity.FindFirst("login").Value;
            return StatusCode(200, _service.Listar(descricao));
        }

        /// <summary>
        /// Através dessa rota você será capaz de listar uma tarefa -
        /// Campos obrigatórios: tarefaiId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Geral)]
        [HttpGet("Tarefa/{tarefaiId}")]
        public IActionResult ObterPorId([FromRoute] int tarefaiId)
        {
            return StatusCode(200, _service.Obter(tarefaiId));
        }

        /// <summary>
        /// Através dessa rota você será capaz de cadastrar uma tarefa - 
        /// Campos obrigatórios: todos os campos são obrigatórios
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Funcionario)]
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
        /// Através dessa rota você será capaz de deletar uma tarefa - 
        /// Campos obrigatórios: tarefaiId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Funcionario)]
        [HttpDelete("Tarefa/{tarefaiId}")]
        public IActionResult Deletar([FromRoute] int tarefaiId)
        {
            _service.Deletar(tarefaiId);
            return StatusCode(200);
        }

        /// <summary>
        /// Através dessa rota você será capaz de atualizar uma tarefa - 
        /// Campos obrigatórios: Não há campos obrigatórios
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Funcionario)]
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
