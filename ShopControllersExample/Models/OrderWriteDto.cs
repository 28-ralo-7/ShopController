namespace ShopControllersExample.Models
{
    public class OrderWriteDto
    {
        public DateTime CreatedAt { get; set; }
        public string Number { get; set; }
        public int? ClientId { get; set; }
    }
}
