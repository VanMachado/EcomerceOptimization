namespace EcomerceOptimization.Domain.Entity
{
    public class UserEcommerce
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string NomeCompleto { get; set; }
        public string Password { get; set; }

        public UserEcommerce()
        {
        }
    }
}
