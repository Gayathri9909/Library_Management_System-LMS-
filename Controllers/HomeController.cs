using library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace library.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Userrepository userRepo = new Userrepository();
                    if (userRepo.AddUser(User))
                    {
                        TempData["SuccessMessage"] = "Registration Successful. Please log in.";
                        return RedirectToAction("Login","Home"); ;
                        
                    }
                   
                    //ModelState.Clear();
                }
                return View(User);
            }
            catch (Exception ex)
            {
                ViewData["message"] = "An error occurred while processing your request.";
                return View();
            }
        }


        // POST: Home/Contact
        [HttpPost]
        public ActionResult Contact(ContactModel Contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactRepository contactRepo = new ContactRepository();
                    if (contactRepo.AddContact(Contact))
                    {
                        TempData["Message"] = "Message submitted successfully!";
                    }
                    else
                    {
                        TempData["Message"] = "Message submitted successfully.";
                    }

                    return RedirectToAction("Contact"); // Redirect to the Contact action
                }

                // If model state is not valid, redisplay the form with validation errors
                return View(Contact);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Contact");
            }
        }

        //login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            if (login == null || !ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid login details.";
                return View();
            }

            string sqlCon = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;

            using (SqlConnection con = new SqlConnection(sqlCon))
            {
                using (SqlCommand cmd = new SqlCommand("GetLogindetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", login.Email);
                    cmd.Parameters.AddWithValue("@Password", login.Password);

                    SqlParameter outputIdParam = new SqlParameter("@Status", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                        int status = (int)outputIdParam.Value;

                        if (status == 0)
                        {
                            if (login != null)
                            {
                                Session["Email"] = login.Email;  // Store email in session
                               
                            }
                            ViewData["Message"] = "Login Successful!";
                            return RedirectToAction("UserPage", "User");
                        }
                        else if (status == 1)
                        {
                            // Invalid email
                            ViewData["Message"] = "Invalid email address.";
                        }
                        else if (status == 2)
                        {
                            // Incorrect password
                            ViewData["Message"] = "Incorrect password.";
                        }
                        else
                        {
                            // Handle unexpected status values
                            ViewData["Message"] = "An unexpected error occurred.";
                        }
                        return View();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception details here
                        ViewData["Message"] = "An error occurred ";
                        return View();
                    }
                }
            }

        }
        //admin login
        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Admin(AdminLogin login)
        {
            if (login == null || !ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid login details.";
                return View();
            }

            string sqlCon = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;

            using (SqlConnection con = new SqlConnection(sqlCon))
            {
                using (SqlCommand cmd = new SqlCommand("Admindetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdminName", login.AdminName);
                    cmd.Parameters.AddWithValue("@AdminPassword", login.AdminPassword);

                    SqlParameter outputIdParam = new SqlParameter("@Status", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                        int status = (int)outputIdParam.Value;

                        if (status == 0)
                        {
                            // Successful login
                            Session["AdminName"] = login.AdminName; // Store admin name in session
                            ViewData["Message"] = "Login Successful!";
                            return RedirectToAction("AdminHome", "Admin");
                        }
                        else if (status == 1)
                        {
                            // Invalid admin name
                            ViewData["Message"] = "Invalid admin name.";
                        }
                        else if (status == 2)
                        {
                            // Incorrect password
                            ViewData["Message"] = "Incorrect password.";
                        }
                        else
                        {
                            // Handle unexpected status values
                            ViewData["Message"] = "An unexpected error occurred.";
                        }
                        return View();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception details here
                        ViewData["Message"] = "An error occurred: " + ex.Message;
                        return View();
                    }
                }
            }


          
        }

        //search operation in booklist

        public ActionResult Booklist(string searchBy, string search)
        {
            BookRepository bookRepo = new BookRepository();
            var books = bookRepo.GetBook();

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(searchBy))
            {
                
                if (searchBy == "Title")
                {
                    books = books.Where(book => book.BookTitle.Trim().Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else if (searchBy == "Category")
                {
                    books = books.Where(book => book.Category.Trim().Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }

            return View(books); 
        }



    }
}