using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace BookStore.Sechadule
{
    public class DailyCallCartJob : IJob
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        SqlConnection con;
        SqlCommand cmd;
        public async Task Execute(IJobExecutionContext context)
        {
            string command = "SP_DeleteCart";
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    using (cmd = new SqlCommand(command, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}