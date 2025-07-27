using BookShoppingCartMvcUI.Data;
using BookShoppingCartMvcUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookShoppingCartMvcUI.Repo
{
    public class CartRepo : ICartRepo
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

     public CartRepo( ApplicationDbContext db , UserManager<IdentityUser> userManager , IHttpContextAccessor httpContextAccessor ) 
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<string> GetUserAsync() 
        {
            var purpoal =  _httpContextAccessor.HttpContext ;

            if (purpoal == null || purpoal.User == null)
                return null;

            var userId = _userManager.GetUserId(purpoal.User);
            return userId ; 
        }

        public async Task<ShoppingCart> GetCartIdAsync(string userId)
{
    if (string.IsNullOrEmpty(userId))
        throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty");

    var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(a => a.UserId == userId);

    if (cart == null)
    {
        cart = new ShoppingCart
        {
            UserId = userId
        };

        await _db.ShoppingCarts.AddAsync(cart); // استخدام AddAsync أفضل
        await _db.SaveChangesAsync();
    }

    return cart;
}




        public async Task<int> GetCartItemCount(string userId = "")
        {
            var user = await GetUserAsync();

            var cart = await GetCartIdAsync(user);

            var itemCount = await _db.CartDetails
                            .Where(cd => cd.ShoppingCartId == cart.Id)
                            .SumAsync(cd => cd.Quantity);

            return itemCount;
        }



        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = await GetUserAsync();
            if (userId == null)
                throw new InvalidOperationException("Invalid userid");
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Book)
                                  .ThenInclude(a => a.Stock)
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Book)
                                  .ThenInclude(a => a.Genre)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }


        public async Task<int> AddItem(int bookId, int qty) 
        {
          var user = await GetUserAsync();


       //      var transaction = _db.Database.BeginTransaction();
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try 
            {   
                
                if (string.IsNullOrEmpty(user)) 
                throw new UnauthorizedAccessException("please log_in first");

                var cart = await GetCartIdAsync(user);   //// improtat


                var cartdetails = await _db.CartDetails.FirstOrDefaultAsync(a => a.BookId == bookId && a.ShoppingCartId == cart.Id);


               if(cartdetails == null) 
                {
                    var book = await _db.Books.FindAsync(bookId);  

                    cartdetails = new CartDetail
                    {
                        BookId = bookId,
                        Quantity = qty,
                        ShoppingCartId = cart.Id,
                        Book = book , 
                        UnitPrice = book.Price 
                    }; 
                    
                  await  _db.CartDetails.AddAsync(cartdetails );
                }
                else 
                {
                    cartdetails.Quantity += qty;

                }

                await _db.SaveChangesAsync();     //////////
                await transaction.CommitAsync();   /////////////
            }
            catch 
            {
                await transaction.RollbackAsync(); ////////////// 
                throw;
            }


            var cartItemCount = await GetCartItemCount(user); ///////// 
            return cartItemCount;

        }


        public async Task<int> RemoveItem(int bookId)
        {
            var user = await GetUserAsync();




            //      var transaction = _db.Database.BeginTransaction();
            try
            {

                if (string.IsNullOrEmpty(user))
                    throw new UnauthorizedAccessException("please log_in first");

                var cart = await GetCartIdAsync(user);   //// improtat

                if (cart == null)
                {
                    throw new InvalidOperationException("Invalid cart");

                }

                var cartdetails = await _db.CartDetails.FirstOrDefaultAsync(a => a.BookId == bookId && a.ShoppingCartId == cart.Id);


               if(cartdetails.Quantity <= 1 ) 
                {
                    _db.CartDetails.Remove(cartdetails);
                    await _db.SaveChangesAsync();
                }
                else 
                {
                    cartdetails.Quantity = cartdetails.Quantity - 1;
                    await _db.SaveChangesAsync();

                }




                await _db.SaveChangesAsync();     //////////
            }
            catch
            {
                throw;
            }


            var cartItemCount = await GetCartItemCount(user); ///////// 
            return cartItemCount;

        }

        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = await GetUserAsync();
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged-in");
                var cart = await GetCartIdAsync(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new InvalidOperationException("Cart is empty");
          
        
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    IsPaid = false,
                    OrderStatus =  OrderStatus.Pending
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);

                    // update stock here

                    var stock = await _db.Stocks.FirstOrDefaultAsync(a => a.BookId == item.BookId);
                    if (stock == null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }

                    if (item.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"Only {stock.Quantity} items(s) are available in the stock");
                    }
                    // decrease the number of quantity from the stock table
                    stock.Quantity -= item.Quantity;
                }
                //_db.SaveChanges();

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

     
        }
    }


