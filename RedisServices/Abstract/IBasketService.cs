using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedisAPI.Models.BaketModels;

namespace RedisServices.Abstract
{
    public interface IBasketService
    {
        Task<ShoppingBasket> GetBasketAsync(string userId);
        Task AddItemToBasket(string userId, BasketItem newItem);
        Task DeleteBasketAsync(string userId);
    }
}
