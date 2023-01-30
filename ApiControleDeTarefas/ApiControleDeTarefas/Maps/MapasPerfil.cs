using ApiControleDePonto.Domain.Models.Contratos;
using ApiControleDeTarefas.Domain.Models;
using AutoMapper;

namespace ApiControleDePonto.Maps
{
    public class MapasPerfil : Profile
    {
        public MapasPerfil()
        {
            #region Entity to Request

            CreateMap<FuncionarioRequest, Funcionario>();



            #endregion

            #region Response to Entity


            #endregion

            #region HttpApi Response to Entity

            //CreateMap<BrasilCep, EnderecoResponse>()
            //    .ForMember(p => p.CEP, map => map.MapFrom(s => s.Cep))
            //    .ForMember(p => p.Rua, map => map.MapFrom(s => s.Street))
            //    .ForMember(p => p.Cidade, map => map.MapFrom(s => s.City))
            //    .ForMember(p => p.Estado, map => map.MapFrom(s => s.State));

            #endregion
        }
    }
}
