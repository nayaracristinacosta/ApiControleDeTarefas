using ApiControleDePonto.Domain.Models.Contratos;
using ApiControleDeTarefas.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Services
{
    public class AutorizacaoService
    {
        private readonly IConfiguration _config;
        private readonly FuncionarioService _usuarioService;
        public AutorizacaoService(FuncionarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _config = configuration;
        }

        public Token Login(FuncionarioRequest model)
        {
            var usuario = _usuarioService.ObterUsuarioPorCredenciais(model.EmailFuncionario, model.SenhaFuncionario);
            if (usuario is null)
                throw new InvalidOperationException("Usuário ou senha inválidos.");

            var senhaJwt = Encoding.ASCII.GetBytes
               (_config["SenhaJwt"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                       new Claim("Email", usuario.EmailDoFuncionario),
                       new Claim(ClaimTypes.Role, Convert.ToString(usuario.Perfil.GetHashCode())),
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(senhaJwt),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            return new Token()
            {
                Bearer = stringToken,
                Validade = tokenDescriptor.Expires.Value,
                NivelAcesso = usuario.Perfil.GetHashCode(),
                NomeUsuario = usuario.NomeDoFuncionario
            };
        }
    }
}
