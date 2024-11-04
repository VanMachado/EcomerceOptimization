using EcomerceOptimization.Domain.Entity.Enum;

namespace EcomerceOptimization.Domain.Entity
{
    public class UserEcommerce
    {
        public int Id { get; set; }
        public RoleId RoleId { get; set; }
        public string NomeCompleto { get; set; }
        public string Password { get; set; }

        public UserEcommerce()
        {
        }
    }
}
