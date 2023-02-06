using ApiControleDeTarefas.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiControleDeTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;
using ApiControleDeTarefas.Domain.Utils;

namespace ApiControleDeTarefas.Controllers
{
    [Authorize]
    [ApiController]
    public class EmpresaClienteController : ControllerBase
    {
        private readonly EmpresaClienteService _service;
        public EmpresaClienteController(EmpresaClienteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Através dessa rota você será capaz de listar as empresas  - 
        /// Campos obrigatórios: Não há campo obrigatorios, razaoSocial é opcional
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpGet("EmpresaCliente")]
        public IActionResult Listar([FromQuery] string? razaoSocial)
        {
            return StatusCode(200, _service.Listar(razaoSocial));
        }

        /// <summary>
        /// Através dessa rota você será capaz de listar uma empresa -
        /// Campos obrigatórios: empresaCleinteId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpGet("EmpresaCliente/{empresaClienteId}")]
        public IActionResult ObterPorId([FromRoute] int empresaClienteId)
        {
            return StatusCode(200, _service.Obter(empresaClienteId));
        }

        /// <summary>
        /// Através dessa rota você será capaz de cadastrar uma empresa - 
        /// Campos obrigatórios: todos os campos são obrigatórios
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpPost("EmpresaCliente")]
        public IActionResult Inserir([FromBody] EmpresaClienteRequest model)
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
        /// Através dessa rota você será capaz de deletar uma emrpesa - 
        /// Campos obrigatórios: empresaClienteId
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpDelete("EmpresaCliente/{empresaClienteId}")]
        public IActionResult Deletar([FromRoute] int empresaClienteId)
        {
            _service.Deletar(empresaClienteId);
            return StatusCode(200);
        }

        /// <summary>
        /// Através dessa rota você será capaz de atualizar uma empresa - 
        /// Campos obrigatórios: Você pode alterar qualquer campo
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        [Authorize(Roles = ConstantUtil.Gerente)]
        [HttpPut("EmpresaCliente")]
        public IActionResult Atualizar([FromBody] EmpresaCliente model)
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
