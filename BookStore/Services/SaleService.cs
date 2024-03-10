using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Services
{
    public class SaleService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        SqlConnection con;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        DataRow row;
        SqlParameter outputParameter;

        public List<SalesModel> GetSales(int? userId=null)
        {
            string command = "SP_GetSales";
            using(con=new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    adapter=new SqlDataAdapter(cmd);
                    dt=new DataTable();
                    adapter.Fill(dt);
                    if(dt.Rows.Count > 0 ) {
                        List<SalesModel> sales = new List<SalesModel>();
                        foreach(DataRow row in dt.Rows)
                        {
                            sales.Add(new SalesModel
                            {
                                UserId = Convert.ToInt32(row["UserId"]),
                                UserEmail = Convert.ToString(row["Email"]),
                                UserPhone = Convert.ToString(row["ContactNumber"]),
                                SalesId = Convert.ToInt32(row["SaleId"]),
                                UserName = Convert.ToString(row["UserName"]),
                                TotalPriced = Convert.ToDecimal(row["UnitTotalPrice"]),
                                SalesDate = Convert.ToDateTime(row["SalesDate"])
                            });
                        }
                        return sales;
                    }
                    return null;
                }
            }  
        }
        public List<SaleDetailesModel> GetSaleDetailes(int saleId)
        {
            string command = "SP_GetSaleDetailes";
            using (con = new SqlConnection(connectionString))
            {
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@saleId", saleId);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        List<SaleDetailesModel> salesDetailes = new List<SaleDetailesModel>();
                        foreach (DataRow row in dt.Rows)
                        {
                            salesDetailes.Add(new SaleDetailesModel
                            {
                                SaleId = Convert.ToInt32(row["SaleId"]),
                                SalesDate = Convert.ToDateTime(row["SalesDate"]),
                                TotalUnitPrice = Convert.ToDecimal(row["TotalUnitPrice"]),
                                Price = Convert.ToDecimal(row["Price"]),
                                SaleQuantity = Convert.ToInt32(row["Quantity"]),
                                BookTitle = Convert.ToString(row["BookTitle"]),
                                CoverImageUrl = Convert.ToString(row["CoverImageUrl"])
                            });
                        }
                        return salesDetailes;
                    }
                    return null;
                }
            }
        }
    }
}
