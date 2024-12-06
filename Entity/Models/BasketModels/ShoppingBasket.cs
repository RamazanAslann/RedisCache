
namespace RedisAPI.Models.BaketModels
{
    public class ShoppingBasket
    {
        public ShoppingBasket()
        {
            BasketId = Guid.NewGuid();
        }

        public Guid BasketId { get; set; }   
        public string UserId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
