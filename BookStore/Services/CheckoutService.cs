using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class CheckoutService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        SqlConnection con;
        SqlCommand cmd;
        public void Checkout(int userId)
        {
            string command = "SP_MakeSaleForUser";
            using (con = new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    con.Open(); 
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
