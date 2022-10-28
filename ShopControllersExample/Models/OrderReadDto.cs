namespace ShopControllersExample.Models
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Number { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<OrderCompositionDto> Position { get; set; }
    }
}
