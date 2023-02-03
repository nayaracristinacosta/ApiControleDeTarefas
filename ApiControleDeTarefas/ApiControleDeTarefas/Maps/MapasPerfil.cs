using ApiControleDePonto.Domain.Models.Contratos;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;
using AutoMapper;

namespace ApiControleDePonto.Maps
{
    public class MapasPerfil : Profile
    {
        public MapasPerfil()
        {
            #region Entity to Request

            CreateMap<FuncionarioRequest, Funcionario>();
            CreateMap<TarefaRequest, Tarefa>();
            CreateMap<EmpresaClienteRequest, EmpresaCliente>();

            #endregion
      
        }
    }
}
