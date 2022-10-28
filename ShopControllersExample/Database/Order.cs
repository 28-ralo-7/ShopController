using System.ComponentModel.DataAnnotations;

namespace ShopControllersExample.Database
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Number { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }


        public List<OrderComposition> Structures { get; set; }
        public Client Client { get; set; }
        public User User { get; set; }
    }
}
