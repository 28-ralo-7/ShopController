using System.ComponentModel.DataAnnotations;

namespace ShopControllersExample.Database
{
    public class OrderStructure
    {
        [Key]
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int GoodsId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }


        public Order Order { get; set; }
        public Goods Goods { get; set; }
    }
}
