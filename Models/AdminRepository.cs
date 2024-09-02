using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class AdminRepository
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["mycon"].ToString();
            con = new SqlConnection(constr);
        }

        //to add admin details
        public bool AddAdmin(AdminModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("InsertAdmindetails", con);
            com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@AdminId", obj.AdminId);
            com.Parameters.AddWithValue("@AdminName", obj.AdminName);
            com.Parameters.AddWithValue("@AdminPassword", obj.AdminPassword);

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

        //to view admin details
        public List<AdminModel> GetAdmin()
        {
            connection();
            List<AdminModel> AdminList = new List<AdminModel>();
            SqlCommand com = new SqlCommand("GetAdmindetails", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();


            AdminList = (from DataRow dr in dt.Rows

                        select new AdminModel()
                        {
                            AdminId = Convert.ToInt32(dr["AdminId"]),
                            AdminName = Convert.ToString(dr["AdminName"]),
                            AdminPassword = Convert.ToString(dr["AdminPassword"])

                        }).ToList();
            return AdminList;
        }

        //to update admindetails
        public bool UpdateAdmin(AdminModel obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateAdmindetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AdminId", obj.AdminId);
            com.Parameters.AddWithValue("@AdminName", obj.AdminName);
            com.Parameters.AddWithValue("@AdminPassword", obj.AdminPassword);

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

        // to delete admin
        public bool DeleteAdmin(int id)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteAdmindetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AdminId", id);

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
     
    }
}