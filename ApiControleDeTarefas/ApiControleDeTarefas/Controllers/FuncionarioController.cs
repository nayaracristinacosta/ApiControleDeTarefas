using ApiControleDeTarefas.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiControleDeTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Utils;

namespace ApiControleDeTarefas.Controllers
{
    [Authorize]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly FuncionarioService _service;
        public FuncionarioController(FuncionarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Através dessa rota você será capaz de listar os funcionários - 
        /// Campos obrigatórios: o campo nomeDoFuncionario é opcional
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpGet("Funcionario")]
        public IActionResult Listar([FromQuery] string? nomeDoFuncionario)
        {
            return StatusCode(200, _service.Listar(nomeDoFuncionario));
        }

        /// <summary>
        /// Através dessa rota você será capaz de listar um funcionário -
        /// Campos obrigatórios: funcionaroiId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Geral)]
        [HttpGet("Funcionario/{funcionaroiId}")]
        public IActionResult ObterPorId([FromRoute] int funcionaroiId)
        {
            return StatusCode(200, _service.Obter(funcionaroiId));
        }

        /// <summary>
        /// Através dessa rota você será capaz de cadastrar um funcionário - 
        /// Campos obrigatórios: todos os campos são obrigatorios
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpPost("Funcionario")]
        public IActionResult Inserir([FromBody] Funcionario model)
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
        /// Através dessa rota você será capaz de deletar um funcionário - 
        /// Campos obrigatórios: funcionaroiId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpDelete("Funcionario/{funcionaroiId}")]
        public IActionResult Deletar([FromRoute] int funcionaroiId)
        {
            _service.Deletar(funcionaroiId);
            return StatusCode(200);
        }

        /// <summary>
        /// Através dessa rota você será capaz de atualizar os dados de um funcionário - 
        /// Campos obrigatórios: os campos são opcionais
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpPut("Funcionario")]
        public IActionResult Atualizar([FromBody] Funcionario model)
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

        /// <summary>
        /// Através dessa rota você será capaz de receber toke no e-mail - 
        /// Não está funcionando
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpPost("Funcionario/{emailFuncionario}")]
        public IActionResult RedefinirSenha([FromRoute] string emailFuncionario)
        {
            _service.EnviaEmail(emailFuncionario);
            return StatusCode(200);
        }
    }
}
