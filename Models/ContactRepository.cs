using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class ContactRepository
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["mycon"].ToString();
            con = new SqlConnection(constr);
        }

        //to add contact details
        public bool AddContact(ContactModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("InsertContactdetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", obj.Name);
            com.Parameters.AddWithValue("@Email", obj.Email);
            com.Parameters.AddWithValue("@Subject", obj.Subject);
            com.Parameters.AddWithValue("@Message", obj.Message);
            

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
        //to view contact details
        public List<ContactModel> GetContact()
        {
            connection();
            List<ContactModel> ContactList = new List<ContactModel>();
            SqlCommand com = new SqlCommand("GetContactdetails", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();


            ContactList = (from DataRow dr in dt.Rows

                        select new ContactModel()
                        {
                            Name = Convert.ToString(dr["Name"]),
                            Email = Convert.ToString(dr["Email"]),
                            Subject= Convert.ToString(dr["Subject"]),
                            Message = Convert.ToString(dr["Message"]),

                        }).ToList();
            return ContactList;
        }
    }
}