using AutoMapper;
using Ids4 = IdentityServer4.Models;
namespace Czar.IdentityServer4.Mappers
{
    /// <summary>
    /// Defines entity/model mapping for persisted grants.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class PersistedGrantMapperProfile:Profile
    {
        /// <summary>
        /// <see cref="PersistedGrantMapperProfile">
        /// </see>
        /// </summary>
        public PersistedGrantMapperProfile()
        {
            CreateMap<Entities.PersistedGrant, Ids4.PersistedGrant>(MemberList.Destination)
                .ReverseMap();
        }
    }
}
