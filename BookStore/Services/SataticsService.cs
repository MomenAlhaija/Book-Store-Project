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
    public class SataticsService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        SqlConnection con;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        DataRow row;
        SqlParameter outputParameter;
        public StaticsModel GetSatics()
        {
            using(con=new SqlConnection(connectionString))
            {
                string command = "SP_Statics";
                using (cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    row = dt.Rows[0];
                    if (dt.Rows.Count > 0)
                    {
                        return new StaticsModel
                        {
                            TotalSales = Convert.ToDecimal(row["CountSales"]),
                            CountUsers = Convert.ToInt32(row["CountUsers"]),
                            CountOrders = Convert.ToInt32(row["CountOrders"]),
                            CountProducts = Convert.ToInt32(row["CountBooks"]),
                            TodaySales=GetTodaySales()
                        };
                    }
                    return null;
                }
            }
        }
        public List<SalesModel> GetTodaySales()
        {
            string command = "SP_GetTodaySales";
            using (con=new SqlConnection(connectionString)) { 
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    List<SalesModel> sales = new List<SalesModel>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows) {
                            sales.Add(new SalesModel
                            {
                                FullName = Convert.ToString(row["FullName"]),
                                TotalPriced = Convert.ToDecimal(row["TotalSales"]),
                                SalesId = Convert.ToInt32(row["SaleId"])
                            });
                        }
                        return sales;

                    }
                    return null;

                }
            }
        }
     }
}
