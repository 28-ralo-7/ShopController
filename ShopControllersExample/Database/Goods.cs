using System.ComponentModel.DataAnnotations;

namespace ShopControllersExample.Database
{
    public class Goods
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
