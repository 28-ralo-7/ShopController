namespace ShopControllersExample.Models
{
    public class OrderCompositionDto
    {
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
