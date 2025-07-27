using BookShoppingCartMvcUI.Models;


namespace BookShoppingCartMvcUI.Repo
{
    public interface IUserOrderRepo
    {
        Task<IEnumerable<Order>> UserOrders(bool getAll = false);
        Task TogglePaymentStatus(int orderId);
        Task<Order?> GetOrderById(int id);
    }
}