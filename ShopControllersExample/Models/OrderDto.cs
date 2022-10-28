namespace ShopControllersExample.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Number { get; set; }
        public int? ClientId { get; set; }
        public int? UserId { get; set; }
    }
}
