using System.Text.Json.Serialization;

namespace EcomerceOptimization.Domain.Entity.DTO
{
    public class OrderEcommerceDTO
    {        
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public double Preco { get; set; }
        public int ClientId { get; set; }
    }
}
