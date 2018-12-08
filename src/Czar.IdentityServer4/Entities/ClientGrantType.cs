
namespace Czar.IdentityServer4.Entities
{
    public class ClientGrantType
    {
        public int Id { get; set; }
        public string GrantType { get; set; }
        public Client Client { get; set; }
    }
}