namespace Czar.IdentityServer4.Entities
{
    public class ApiResourceClaim : UserClaim
    {
        public ApiResource ApiResource { get; set; }
    }
}