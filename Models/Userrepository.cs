using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class Userrepository
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["mycon"].ToString();
            con = new SqlConnection(constr);
        }

        //to add user details
        public bool AddUser(UserModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("InsertRegisterdetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);
            com.Parameters.AddWithValue("@DOB", Convert.ToDateTime(obj.DateOfBirth));
            com.Parameters.AddWithValue("@Email", obj.Email);
            com.Parameters.AddWithValue("@Password", obj.Password);
            com.Parameters.AddWithValue("@Confirmpassword", obj.ConfirmPassword);
            com.Parameters.AddWithValue("@Phonenumber", obj.PhoneNumber);
            com.Parameters.AddWithValue("@State", obj.State);
            com.Parameters.AddWithValue("@District", obj.District);
            com.Parameters.AddWithValue("@Address", obj.Address);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }

        }

        //to view user details
        public List<UserModel> GetUser()
        {
            connection();
            List<UserModel> UserList = new List<UserModel>();
            SqlCommand com = new SqlCommand("GetRegisterdetails", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            UserList = (from DataRow dr in dt.Rows

                        select new UserModel()
                        {
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            DateOfBirth= Convert.ToDateTime(dr["DOB"]),
                            Email = Convert.ToString(dr["Email"]),
                            Password = Convert.ToString(dr["Password"]),
                            ConfirmPassword = Convert.ToString(dr["Confirmpassword"]),
                            PhoneNumber = Convert.ToString(dr["Phonenumber"]),
                            State = Convert.ToString(dr["State"]),
                            District = Convert.ToString(dr["District"]),
                            Address = Convert.ToString(dr["Address"]),

                        }).ToList();
            return UserList;
        }

        //to update userdetails
        public bool UpdateUser(UserModel obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateRegisterdetails", con);
            com.CommandType = CommandType.StoredProcedure;
  
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);
            com.Parameters.AddWithValue("@DOB", obj.DateOfBirth);
            com.Parameters.AddWithValue("@Email", obj.Email);
            com.Parameters.AddWithValue("@Password", obj.Password);
            com.Parameters.AddWithValue("@Confirmpassword", obj.ConfirmPassword);
            com.Parameters.AddWithValue("@Phonenumber", obj.PhoneNumber);
            com.Parameters.AddWithValue("@State", obj.State);
            com.Parameters.AddWithValue("@District", obj.District);
            com.Parameters.AddWithValue("@Address", obj.Address);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }

        // to delete user
        public bool DeleteUser(string email)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteRegisterdetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Email", email);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }

        //for user profile edit in user page
        public UserModel GetUserByEmail(string email)
        {
            UserModel user = null;
            string connectionString = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetUserByEmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserModel
                            {
                                // Populate the UserModel properties
                             
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DOB")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                State = reader.GetString(reader.GetOrdinal("State")),
                                District = reader.GetString(reader.GetOrdinal("District")),
                                Address = reader.GetString(reader.GetOrdinal("Address"))
                            };
                        }
                    }
                }
            }

            return user;
        }
      

       

    }
}