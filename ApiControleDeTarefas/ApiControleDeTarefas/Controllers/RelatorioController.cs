using ApiControleDeTarefas.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiControleDeTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;
using System.Security.Claims;


namespace ApiControleDeTarefas.Controllers
{
    [Authorize]
    [ApiController]
    public class RelarorioController : ControllerBase
    {
        private readonly RelatorioService _service;
        public RelarorioController(RelatorioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Através dessa rota você será capaz de listar um cargo - 
        /// Campos obrigatórios: descricao
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucesso, e retorna o elemento encontrado via ID</response>
        //[Authorize(Roles = "1,2,3")]
        [HttpGet("Relatorio")]
        public IActionResult Listar([FromQuery] Filtro filtro)
        {
            return StatusCode(200, _service.ObterRelatorioTarefa(new RelatorioRequest
            {
                Filtro = filtro
            })); 
        }

    }
}
