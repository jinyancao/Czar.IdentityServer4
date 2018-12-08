using AutoMapper;
using Ids4 = IdentityServer4.Models;
namespace Czar.IdentityServer4.Mappers
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 客户端信息映射
    /// </summary>
    public static class ClientMappers
    {
        static ClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Ids4.Client ToModel(this Entities.Client entity)
        {
            return Mapper.Map<Ids4.Client>(entity);
        }

        public static Entities.Client ToEntity(this Ids4.Client model)
        {
            return Mapper.Map<Entities.Client>(model);
        }
    }
}