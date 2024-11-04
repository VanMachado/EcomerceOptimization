namespace EcomerceOptimization.Domain.Entity
{
    public class OrderEcommerce
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public double Preco { get; set; }
        public int ClientId { get; set; }

        public OrderEcommerce()
        {
        }
    }
}
