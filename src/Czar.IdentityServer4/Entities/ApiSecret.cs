namespace Czar.IdentityServer4.Entities
{
    public class ApiSecret : Secret
    {
        public ApiResource ApiResource { get; set; }
    }
}