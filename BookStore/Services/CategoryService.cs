using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BookStore.Services
{
    public class CategoryService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        SqlConnection con;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        DataRow row;
        SqlParameter outputParameter;
        public List<CategoryModel> GetCategories()
        {
            using (con = new SqlConnection(connectionString))
            { 
                string command = "Select*From Categories where IsDeleted=0";
                using (cmd = new SqlCommand(command, con))
                {
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    List<CategoryModel> categories = new List<CategoryModel>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        categories.Add(new CategoryModel
                        {
                            CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            CategoryName = dr["CategoryName"].ToString(),
                            CategoryDescription = Convert.ToString(dr["CategoryDesc"]),
                        });
                    }
                    return categories;
                }
            }
        }
        public CategoryModel GetCategorieById(int categoryId)
        {
            using (con = new SqlConnection(connectionString))
            {
                string command = "Select*From Categories where CategoryId=@categoryId and IsDeleted=0";
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        return new CategoryModel
                        {
                            CategoryId = Convert.ToInt32(row["CategoryId"]),
                            CategoryName = row["CategoryName"].ToString(),
                            CategoryDescription = Convert.ToString(row["CategoryDesc"]),
                        };
                    }
                    return null;
                }
            }
        }
        public void AddCategory(CategoryModel category)
        {
            string command = "SP_AddOrUpdateCategory";
            using (con = new SqlConnection(connectionString))
            {
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@categoryId", category.CategoryId);
                    cmd.Parameters.AddWithValue("@categoryName", category.CategoryName);
                    cmd.Parameters.AddWithValue("@categoryDesc", category.CategoryDescription);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void EditCategory(CategoryModel category)
        {
            string command = "SP_AddOrUpdateCategory";
            using (con = new SqlConnection(connectionString))
            {
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@categoryId", category.CategoryId);
                    cmd.Parameters.AddWithValue("@categoryName", category.CategoryName);
                    cmd.Parameters.AddWithValue("@categoryDesc", category.CategoryDescription);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteCategory(int? categoryId)
        {
            string command = "SP_DeleteCategory";
            using (con = new SqlConnection(connectionString))
            {
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@categoryId",categoryId.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
