using ApiControleDeTarefas.Repositories.Repositorio;
using ApiControleDeTarefas.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc
{
    public class ContainerDependencia
    {
        public static void RegistrarServicos(IServiceCollection services)
        {
            //repositorios
            services.AddScoped<FuncionarioRepositorio, FuncionarioRepositorio>();
            services.AddScoped<EmpresaClienteRepositorio, EmpresaClienteRepositorio>();
            services.AddScoped<TarefaRepositorio, TarefaRepositorio>();
           
            //services
            services.AddScoped<FuncionarioService, FuncionarioService>();
            services.AddScoped<EmpresaClienteService, EmpresaClienteService>();
            services.AddScoped<TarefaService, TarefaService>();          
            services.AddScoped<AutorizacaoService, AutorizacaoService>();
            services.AddScoped<RelatorioService, RelatorioService>();
        }
    }
}
