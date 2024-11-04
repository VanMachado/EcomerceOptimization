using System.Text.Json.Serialization;

namespace EcomerceOptimization.Domain.Entity.DTO
{
    public class ClientEcommerceDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
    }
}
