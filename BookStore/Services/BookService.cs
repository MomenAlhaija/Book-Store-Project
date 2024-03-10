using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Web.WebPages;
using System.Reflection;

namespace BookStore.Services
{
    public class BookService
    {

        string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        SqlConnection con;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        DataRow row;
        SqlParameter outputParameter;
        int rate;
        object publishDateValue;
        public void AddOrUpdateBook(AddOrUpdateBookModel bookModel)
        {
            string command = "SP_AddOrUpdateBook";
            using(con=new SqlConnection(connectionString))
            {
                DateTime? PulishDate = bookModel.Published != null && bookModel.Published.Value != DateTime.MinValue ?bookModel.Published.Value : DateTime.MinValue;
                using (cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bookId", bookModel.BookId);
                    cmd.Parameters.AddWithValue("@bookTitle",bookModel.BookTitle);
                    cmd.Parameters.AddWithValue("@subject", bookModel.Subject);
                    cmd.Parameters.AddWithValue("@publishHouse", bookModel.Publisher);
                    cmd.Parameters.AddWithValue("@author", bookModel.Author);
                    cmd.Parameters.AddWithValue("@publishDate", PulishDate.Value);
                    cmd.Parameters.AddWithValue("@coverImageUrl", bookModel.CoverImageUrl);
                    cmd.Parameters.AddWithValue("@quantityInStore", bookModel.QuantityInStore);
                    cmd.Parameters.AddWithValue("@categoryId", bookModel.CategoryId);
                    cmd.Parameters.AddWithValue("@price", bookModel.Price);
                    cmd.Parameters.AddWithValue("@description", bookModel.Description);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public BookModel GetBookDetailes(int bookId)
        {
            using (con = new SqlConnection(connectionString))
            {
                string command = "SP_GetBookDetailes";
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row= dt.Rows[0];
                        return new BookModel
                        {
                            Category = new CategoryModel
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"]),
                                CategoryName = Convert.ToString(row["CategoryName"]),
                                CategoryDescription = Convert.ToString(row["CategoryDesc"])
                            },
                            BookId = Convert.ToInt32(row["BookId"]),
                            BookTitle = Convert.ToString(row["BookTitle"]),
                            Author = Convert.ToString(row["Author"]),
                            Subject = Convert.ToString(row["Subject"]),
                            publishDateValue =row["PublishDate"]!=DBNull.Value? (DateTime)row["PublishDate"]:DateTime.MinValue,
                            PublishDate = publishDateValue != DBNull.Value ? Convert.ToDateTime(publishDateValue) : (DateTime?)null,
                            PublishHouse = Convert.ToString(row["PublishHouse"]),
                            CoverImageUrl = Convert.ToString(row["CoverImageUrl"]),
                            QuantityInStore = Convert.ToInt32(row["QuantityInStore"]),
                            Price = Convert.ToDecimal(row["Price"]),
                            Description = Convert.ToString(row["Description"]),
                            Rate = Convert.IsDBNull(row["BookRate"]) ? 0 : Convert.ToInt32(row["BookRate"])
                        };
                      
                    }
                    return null;
                }
            }
        }
        public List<BasicBookInfoModel> GetBasicInfoBooks()
        {
            using (con = new SqlConnection(connectionString))
            {
                string command = "SP_GetBooksForAdmin";
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        List<BasicBookInfoModel> books = new List<BasicBookInfoModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            books.Add(new BasicBookInfoModel
                            {

                                BookId = Convert.ToInt32(dr["BookId"]),
                                BookTitle = Convert.ToString(dr["BookTitle"]),
                                Author = Convert.ToString(dr["Author"]),
                                Price = Convert.ToDecimal(dr["Price"]),
                                Subject = Convert.ToString(dr["Subject"]),
                                CategoryName = Convert.ToString(dr["CategoryName"]),
                                CoverImageUrl = Convert.ToString(dr["CoverImageUrl"])
                            });
                        }
                        return books;
                    }
                    return null;
                }
            }
        }
        public BookModel GetBookByTitle(string title)
        {
            var command = "Select*from Books where BookTitle=@title and IsDeleted=0";
            using(con=new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.Parameters.AddWithValue("@title", title.Trim());
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        return new BookModel
                        {
                            Category = new CategoryModel
                            {
                                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            },
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookTitle = Convert.ToString(dr["BookTitle"]),
                            Author = Convert.ToString(dr["Author"]),
                            Subject = Convert.ToString(dr["Subject"]),
                            PublishDate = publishDateValue != DBNull.Value ? Convert.ToDateTime(publishDateValue) : (DateTime?)null,
                            PublishHouse = Convert.ToString(dr["PublishHouse"]),
                            CoverImageUrl = Convert.ToString(dr["CoverImageUrl"]),
                            QuantityInStore = Convert.ToInt32(dr["QuantityInStore"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Description = Convert.ToString(dr["Description"]),
                        };
                    }
                    return null;
                }
            }
        }
        public List<BookModel> GetBooksByCategoryId(int? categoryId=0)
        {
            using (con = new SqlConnection(connectionString))
            {
                string command ="SP_GetBooks";
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@categoryId", categoryId != null ? categoryId :int.Parse( null));
                    cmd.Parameters.Add("@bookId", null);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    List<BookModel> books = new List<BookModel>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        books.Add(new BookModel
                        {
                            Category = new CategoryModel
                            {
                                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                CategoryName = Convert.ToString(dr["CategoryName"]),
                                CategoryDescription = Convert.ToString(dr["CategoryDesc"])
                            },
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookTitle = Convert.ToString(dr["BookTitle"]),
                            Author = Convert.ToString(dr["Author"]),
                            Subject = Convert.ToString(dr["Subject"]),
                            PublishDate = publishDateValue != DBNull.Value ? Convert.ToDateTime(publishDateValue) : (DateTime?)null,
                            PublishHouse = Convert.ToString(dr["PublishHouse"]),
                            CoverImageUrl = Convert.ToString(dr["CoverImageUrl"]),
                            QuantityInStore = Convert.ToInt32(dr["QuantityInStore"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Description = Convert.ToString(dr["Description"]),
                            Rate = Convert.IsDBNull(dr["BookRate"]) ? 0 : Convert.ToInt32(dr["BookRate"])
                        });
                    }
                    return books;
                }
            }
        }
        public List<BookModel> GetRequiredBooks(int? categoryId = 0)
        {
            using (con = new SqlConnection(connectionString))
            {
                string command = "SP_GetReguireBooks";
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    List<BookModel> books = new List<BookModel>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        books.Add(new BookModel
                        {
                            Category = new CategoryModel
                            {
                                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                CategoryName = Convert.ToString(dr["CategoryName"]),
                                CategoryDescription = Convert.ToString(dr["CategoryDesc"])
                            },
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookTitle = Convert.ToString(dr["BookTitle"]),
                            Author = Convert.ToString(dr["Author"]),
                            Subject = Convert.ToString(dr["Subject"]),
                            PublishDate = Convert.ToDateTime(dr["PublishDate"]),
                            PublishHouse = Convert.ToString(dr["PublishHouse"]),
                            CoverImageUrl = Convert.ToString(dr["CoverImageUrl"]),
                            QuantityInStore = Convert.ToInt32(dr["QuantityInStore"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Description = Convert.ToString(dr["Description"]),
                        });
                    }
                    return books;
                }
            }
        }

        public void RateBook(RateBookModel rate)
        {
            string command = "SP_RateOnBook";
            using(con=new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", rate.UserId);
                    cmd.Parameters.AddWithValue("@bookId",rate.BookId);
                    cmd.Parameters.AddWithValue("@rate", rate.Rate);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public BookModel GetBookById(int bookId)
        {
            using (con = new SqlConnection(connectionString))
            {
                string command =  "SP_GetBooks";
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@bookId", bookId);
                    cmd.Parameters.Add("@categoryId", 0);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        bool rateParsed = row["BookRate"] != DBNull.Value && int.TryParse(row["BookRate"].ToString(), out rate);
                        return new BookModel
                        {
                            Category = new CategoryModel
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"]),
                                CategoryName = Convert.ToString(row["CategoryName"]),
                                CategoryDescription = Convert.ToString(row["CategoryDesc"])
                            },
                            BookId = Convert.ToInt32(row["BookId"]),
                            BookTitle = Convert.ToString(row["BookTitle"]),
                            Author = Convert.ToString(row["Author"]),
                            Subject = Convert.ToString(row["Subject"]),
                            PublishDate = Convert.ToDateTime(row["PublishDate"]),
                            PublishHouse = Convert.ToString(row["PublishHouse"]),
                            CoverImageUrl = Convert.ToString(row["CoverImageUrl"]),
                            QuantityInStore = Convert.ToInt32(row["QuantityInStore"]),
                            Price = Convert.ToDecimal(row["Price"]),
                            Description = Convert.ToString(row["Description"]),
                            Rate = rateParsed ? rate : (int?)null 
                        };


                    }
                    return null;
                }
            }
        }
        public bool CheckUserPurchasedBook(int bookId, int userId)
        {
            bool purchased = false;
            string command = "SP_CheckUserPurchasedBook";
            using (con= new SqlConnection(connectionString))
            {
                using (cmd= new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@bookId", SqlDbType.Int).Value = bookId;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@purchased", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    purchased = (bool)cmd.Parameters["@purchased"].Value;
                }
            }
            return purchased;
        }
        public bool CheckUserVotedOnBook(int bookId, int userId)
        {
            bool voted = false;
            string command = "SP_UserISVotedOnBook";
            using (con = new SqlConnection(connectionString))
            {
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@bookId", SqlDbType.Int).Value = bookId;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@voted", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    voted = (bool)cmd.Parameters["@voted"].Value;
                }
            }
            return voted;
        }
        public List<VoteModel> GetRatesForBook(int bookId)
        {
            using (con = new SqlConnection(connectionString))
            {
                string command = "SP_GetVoting";
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@bookId", bookId);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        List<VoteModel> Rates = new List<VoteModel>();
                        foreach(DataRow row in  dt.Rows)
                        {
                            Rates.Add(new VoteModel
                            {
                                UserFullName =Convert.ToString(row["FullName"]),
                                BookId=bookId,
                                Rate = Convert.ToInt32(row["Rate"])
                            });
                        }
                        return Rates;
                    }
                    return null;
                }
            }
        }

        public void DeleteBook(int bookId)
        {
            string command = "SP_DeleteBook";
            using(con=new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bookId",bookId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            } 
        }
        public CountDownModel GetCountDown()
        {
            string command = "SP_GetCountDown";
            using(con=new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        return new CountDownModel
                        {
                            CountAuthors = Convert.ToInt32(row["CountAuthors"]),
                            CountBooks = Convert.ToInt32(row["CountBooks"]),
                            CountCategory = Convert.ToInt32(row["CountCategory"]),
                            CountUsers = Convert.ToInt32(row["CountUsers"])
                        };
                    }
                    return null;
                }
            }
        }
    }
}
