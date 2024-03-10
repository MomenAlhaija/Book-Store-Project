using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using BookStore.Models;

namespace BookStore.Services
{
    public class UserService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        SqlConnection con;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        DataRow row;
        SqlParameter outputParameter;
        public UserModel GetUserByEmail(string email)
        {
            if(email is null)
                throw new ArgumentNullException(nameof(email));
            using(con=new SqlConnection(connectionString))
            {
                string command = "Select*From Users Where Email=@email and IsDelete=0";
                using(cmd=new SqlCommand(command, con)){
                    cmd.Parameters.Add("@email", email);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        return new UserModel
                        {
                            UserId = Convert.ToInt32(row["UserId"]),
                            Email = row["Email"].ToString(),
                            UserType = Convert.ToInt32(row["UserType"]),
                            UserName = (row["UserName"]).ToString(),
                            Password = row["Password"].ToString(),
                            Gender = Convert.ToInt32(row["GenderId"]),
                            BirthDate = (DateTime.Parse((row["BirthDate"].ToString()))),
                            FirstName = (row["FirstName"]).ToString(),
                            LastName = (row["LastName"]).ToString(),
                            Phone = (row["ContactNumber"]).ToString()
                        };
                    }
                    return null;
                }
            }

        }
        public UserModel GetUserById(int userId)
        {
            using (con = new SqlConnection(connectionString))
            {
                string command = "Select*From Users Where UserId=@userId and IsDelete=0";
                using (cmd = new SqlCommand(command, con))
                {
                    cmd.Parameters.Add("@userId",userId);
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    row = dt.Rows[0];
                    return new UserModel
                    {
                        UserId = Convert.ToInt32(row["UserId"]),
                        Email = row["Email"].ToString(),
                        UserType = Convert.ToInt32(row["UserType"]),
                        UserName = (row["UserName"]).ToString(),
                        Password = row["Password"].ToString(),
                        Gender = Convert.ToInt32(row["GenderId"]),
                        BirthDate = (DateTime.Parse((row["BirthDate"].ToString()))),
                        FirstName = (row["FirstName"]).ToString(),
                        LastName = (row["LastName"]).ToString(),
                        Phone = (row["ContactNumber"]).ToString()
                    };
                }
            }
        }
        public List<RegisterUserModel> GetUsers()
        {
            using (con = new SqlConnection(connectionString))
            {
                string command = "Select*From Users where IsDelete=0";
                using (cmd = new SqlCommand(command, con))
                {
                    adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    List<RegisterUserModel> users = new List<RegisterUserModel>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        users.Add(new RegisterUserModel
                        {
                            UserId = Convert.ToInt32(dr["UserId"]),
                            Email = dr["Email"].ToString(),
                            UserType = Convert.ToInt32(dr["UserType"]),
                            UserName = (dr["UserName"]).ToString(),
                            Password = dr["Password"].ToString(),
                            Gender =Convert.ToInt32(dr["GenderId"]),
                            BirthDate = (DateTime.Parse((dr["BirthDate"].ToString()))),
                            FirstName = (dr["FirstName"]).ToString(),
                            LastName = (dr["LastName"]).ToString(),
                            Phone = (dr["ContactNumber"]).ToString()
                        });
                    }

                    return users;
                }
            }
        }
        public int? AddOrUpdateUser(RegisterUserModel user)
        {
            using(con=new SqlConnection(connectionString))
            {
                string command = "SP_CreateOrUpdateUser";
                using(cmd=new SqlCommand(command, con)) { 
                
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userIdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@userId", user.UserId != null ? user.UserId : null);
                    cmd.Parameters.Add("@firstName", user.FirstName);
                    cmd.Parameters.Add("@lastName", user.LastName);
                    cmd.Parameters.Add("@birthDate", user.BirthDate);
                    cmd.Parameters.Add("@gender", user.Gender);
                    cmd.Parameters.Add("@password", user.Password);
                    cmd.Parameters.Add("@contactNumber", user.Phone);
                    cmd.Parameters.Add("@userName", user.UserName);
                    cmd.Parameters.Add("@email", user.Email);
                    cmd.Parameters.Add("@userType", user.UserType != null && user.UserType!=0 ? user.UserType : (int)(UserTypeEnum.Customer));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    int? userId = user.UserId != null ? user.UserId : Convert.ToInt32(cmd.Parameters["@userIdOut"].Value);
                    return userId;
                }
            }
        }
        public void ChangePassword(string password,int userId)
        {
            string command = "Update Users set Password=@password where UserId=@userId and IsDelete=0";
            using(con=new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteUser(int userId)
        {
            string command = "SP_DeleteUser";
            using (con = new SqlConnection(connectionString))
            {
                using(cmd=new SqlCommand(command, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId",userId); 
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
