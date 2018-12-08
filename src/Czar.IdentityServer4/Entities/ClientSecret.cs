

namespace Czar.IdentityServer4.Entities
{
    public class ClientSecret : Secret
    {
        public Client Client { get; set; }
    }
}