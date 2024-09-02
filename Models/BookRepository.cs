using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class BookRepository
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["mycon"].ToString();
            con = new SqlConnection(constr);
        }

        //to add book details
        public bool AddBook(BookModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("InsertBookdetails", con);
            com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@BookId", obj.BookId);
            com.Parameters.AddWithValue("@BookTitle", obj.BookTitle);
            com.Parameters.AddWithValue("@Author", obj.Author);
            com.Parameters.AddWithValue("@NumberOfBooks", obj.NumberOfBooks);
            com.Parameters.AddWithValue("@Category", obj.Category);
            com.Parameters.AddWithValue("@ISBN", obj.ISBN);
            com.Parameters.AddWithValue("@PublishYear", obj.PublishYear);

         

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

        //to view book details
        public List<BookModel> GetBook()
        {
            connection();
            List<BookModel> BookList = new List<BookModel>();
            SqlCommand com = new SqlCommand("GetBookdetails", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            
            BookList = (from DataRow dr in dt.Rows

                        select new BookModel()
                        {
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookTitle = Convert.ToString(dr["BookTitle"]),
                            Author = Convert.ToString(dr["Author"]),
                            NumberOfBooks = Convert.ToInt32(dr["NumberOfBooks"]),
                            Category = Convert.ToString(dr["Category"]),
                            ISBN = dr["ISBN"] != DBNull.Value ? Convert.ToString(dr["ISBN"]) : null,
                            PublishYear=Convert.ToInt32(dr["PublishYear"]),
                            
                            IsSelected = false

                        }).ToList();
            return BookList;
        }

        //to update bookdetails
        public bool UpdateBook(BookModel obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateBookdetails", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@BookId", obj.BookId);
            com.Parameters.AddWithValue("@BookTitle", obj.BookTitle);
            com.Parameters.AddWithValue("@Author", obj.Author);
            com.Parameters.AddWithValue("@NumberOfBooks", obj.NumberOfBooks);
            com.Parameters.AddWithValue("@Category", obj.Category);
            com.Parameters.AddWithValue("@ISBN", obj.ISBN); 
            com.Parameters.AddWithValue("@PublishYear", obj.PublishYear);
            

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

        // to delete book details
        public bool DeleteBook(int id)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteBookdetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookId", id);

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

        //for storing book request
        public bool StoreBookRequestInDatabase(BookModel book, string userEmail)
        {
            connection();
            SqlCommand com = new SqlCommand("RequestBookdetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookId", book.BookId);
            com.Parameters.AddWithValue("@BookTitle", book.BookTitle);
            com.Parameters.AddWithValue("@Author", book.Author);
            com.Parameters.AddWithValue("@NumberOfBooks", book.NumberOfBooks);
            com.Parameters.AddWithValue("@Category", book.Category);
            com.Parameters.AddWithValue("@ISBN", book.ISBN);
            com.Parameters.AddWithValue("@PublishYear", book.PublishYear);
            com.Parameters.AddWithValue("@UserEmail", userEmail);


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

        //request book read
        public List<BookModel> RequestBook(string userEmail)
        {
            connection();
            List<BookModel> BookList = new List<BookModel>();
            using (SqlCommand com = new SqlCommand("GetRequestBookdetails", con))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserEmail", userEmail);

                using (SqlDataAdapter da = new SqlDataAdapter(com))
                {
                    DataTable dt = new DataTable();
                    con.Open();
                    da.Fill(dt);
                    con.Close();

                    BookList = (from DataRow dr in dt.Rows
                                select new BookModel()
                                {
                                    BookId = Convert.ToInt32(dr["BookId"]),
                                    BookTitle = Convert.ToString(dr["BookTitle"]),
                                    Author = Convert.ToString(dr["Author"]),
                                    NumberOfBooks = Convert.ToInt32(dr["NumberOfBooks"]),
                                    Category = Convert.ToString(dr["Category"]),
                                    ISBN = Convert.ToString(dr["ISBN"]),
                                    PublishYear = Convert.ToInt32(dr["PublishYear"]),
                                }).ToList();
                }
            }
            return BookList;
        }


        //to view the response details
        public List<BookModel> GetResponse(string userEmail)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;

            using (var con = new SqlConnection(connectionString))
            {

                var responseList = new List<BookModel>();

                using (var com = new SqlCommand("GetResponseDetails", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UserEmail", userEmail);
                    var da = new SqlDataAdapter(com);
                    var dt = new DataTable();

                    con.Open();
                    da.Fill(dt);
                    con.Close();


                    responseList = (from DataRow dr in dt.Rows
                                    select new BookModel()
                                    {
                                        BookId = Convert.ToInt32(dr["BookId"]),
                                        BookTitle = Convert.ToString(dr["BookTitle"]),
                                        Author = Convert.ToString(dr["Author"]),
                                        Category = Convert.ToString(dr["Category"]),
                                        Status=Convert.ToString(dr["RequestStatus"])
                                    }).ToList();
                }

                return responseList;
            }
        }
        //for storing book response
        public bool StoreBookResponseInDatabase(BookModel book, string userEmail, string requestStatus)
        {
            connection();
            SqlCommand com = new SqlCommand("InsertAcceptRequestDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookId", book.BookId);
            com.Parameters.AddWithValue("@BookTitle", book.BookTitle);
            com.Parameters.AddWithValue("@Author", book.Author);
            com.Parameters.AddWithValue("@NumberOfBooks", book.NumberOfBooks);
            com.Parameters.AddWithValue("@Category", book.Category);
            com.Parameters.AddWithValue("@ISBN", book.ISBN);
            com.Parameters.AddWithValue("@PublishYear", book.PublishYear);
            com.Parameters.AddWithValue("@UserEmail", userEmail);
            com.Parameters.AddWithValue("@RequestStatus", requestStatus);

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
        // Request book details by user email
        public List<BookModel> RequestBooks()
        {
            connection();
            List<BookModel> BookList = new List<BookModel>();
            SqlCommand com = new SqlCommand("GetRequestBookdetailsByEmail", con); // Use stored procedure that accepts email
            com.CommandType = CommandType.StoredProcedure;
           
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            BookList = (from DataRow dr in dt.Rows
                        select new BookModel()
                        {
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookTitle = Convert.ToString(dr["BookTitle"]),
                            Author = Convert.ToString(dr["Author"]),
                            NumberOfBooks = Convert.ToInt32(dr["NumberOfBooks"]),
                            Category = Convert.ToString(dr["Category"]),
                            ISBN = dr["ISBN"] != DBNull.Value ? Convert.ToString(dr["ISBN"]) : null,
                            PublishYear = Convert.ToInt32(dr["PublishYear"]),
                            UserEmail = Convert.ToString(dr["UserEmail"]),

                        }).ToList();
            return BookList;
        }


    }
}