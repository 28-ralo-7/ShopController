using System.ComponentModel.DataAnnotations;

namespace ShopControllersExample.Database
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
