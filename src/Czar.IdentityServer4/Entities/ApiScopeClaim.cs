
namespace Czar.IdentityServer4.Entities
{
    public class ApiScopeClaim : UserClaim
    {
        public ApiScope ApiScope { get; set; }
    }
}