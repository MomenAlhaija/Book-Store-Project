using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models;
using System.Net.Configuration;
using System.Security.Cryptography;

namespace BookStore.Services
{
    public class CartService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        SqlConnection con;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        DataRow row;
        SqlParameter outputParameter;
        decimal rate;
        BookService bookService=new BookService();
        public void AddToCart(AddToCartModel cart)
        {
            using(con=new SqlConnection(connectionString))
            {
                string command = "SP_AddToCart";
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId",cart.UserId);
                    cmd.Parameters.AddWithValue("@bookId", cart.BookId);
                    cmd.Parameters.AddWithValue("@quantity", cart.Quantiity);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<CartItemsModel> GetCartItems(int userId) {

            string command = "SP_SelectCartItems";
            using (con = new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        List<CartItemsModel> cartItems = new List<CartItemsModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            cartItems.Add(new CartItemsModel
                            {
                                CartId = Convert.ToInt32(dr["CartId"]),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                BookId = Convert.ToInt32(dr["BookId"]),
                                Price = Convert.ToDecimal(dr["Price"]),
                                BookTitle = Convert.ToString(dr["BookTitle"]),
                                CoverImageUrl = Convert.ToString(dr["CoverImageUrl"]),
                                DetaileId = Convert.ToInt32(dr["DetaileId"])

                            });
                        }

                        return cartItems;
                    }
                    return null;
                }
            }
        
        }
        public void RemoveFromCart(int detaileId)
        {
            string command = "Delete from CartDetails where DetaileId=@detileId";
            using(con=new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con)) {
                    cmd.Parameters.AddWithValue("@detileId", detaileId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public decimal GetTotalPriceForItem(int itemId)
        {
            string command = "select*from CartDetails where DetaileId=@detileId";
            using(con=new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.Parameters.AddWithValue("@detileId", itemId);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    row = dt.Rows[0];
                    var cartItem=new CartItemsModel
                    {
                        CartId = Convert.ToInt32(row["CartId"]),
                        Quantity = Convert.ToInt32(row["Quantity"]),
                        BookId = Convert.ToInt32(row["BookId"]),

                    };
                    var book = bookService.GetBookById(cartItem.BookId);
                    return book.Price * cartItem.Quantity;
                }
            }
        }
        public void UpdateQuantity(int detaileId,int quantity)
        {
            string command = "SP_UpdateQuantity";
            using (con = new SqlConnection(connectionString))
            {
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@detaileId", detaileId);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public decimal? GetTotalPrice(int userId)
        {
            string command = "SP_GetTotalPrice";
            using (con = new SqlConnection(connectionString))
            {
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        object totalPriceObject = row["TotalPrice"];
                        if (totalPriceObject != DBNull.Value)
                        {
                            return Convert.ToDecimal(totalPriceObject);
                        }
                    }
                    return null;
                }
            }
        }
    }
}
