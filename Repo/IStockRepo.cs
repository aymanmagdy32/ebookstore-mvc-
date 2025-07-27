using BookShoppingCartMvcUI.Models;

namespace BookShoppingCartMvcUI.Repo
{
    public interface IStockRepo
    {
        /// <summary>
        /// Retrieves stock records with optional filtering by book name.
        /// </summary>
        /// <param name="sTerm">Search term for book name filtering (optional).</param>
        /// <returns>A list of StockDisplayModel instances.</returns>
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");

        /// <summary>
        /// Retrieves the stock record associated with a specific book ID.
        /// </summary>
        /// <param name="bookId">The ID of the book.</param>
        /// <returns>The stock record if found, otherwise null.</returns>
        Task<Stock?> GetStockByBookId(int bookId);

        /// <summary>
        /// Adds a new stock record or updates the quantity of an existing one.
        /// </summary>
        /// <param name="stockToManage">StockDTO containing book ID and quantity.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task ManageStock(StockDTO stockToManage);
    }
}
