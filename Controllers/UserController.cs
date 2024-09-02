using library.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace library.Controllers
{


    public class UserController : Controller
    {

        // GET: User
        public ActionResult UserPage()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details()
        {
            Userrepository userRepo = new Userrepository();
            ModelState.Clear();
            return View(userRepo.GetUser());
        }


        // GET: User/EditProfile
        public ActionResult EditProfile()
        {
            // Retrieve the email from the session
            string userEmail = Session["Email"] as string;
            if (userEmail == null)
            {
                // Redirect to login if email is not found in the session
                return RedirectToAction("Login", "Home");
            }
            Userrepository userRepo = new Userrepository();

            // Fetch user details by email
            var user = userRepo.GetUserByEmail(userEmail);
            if (user == null)
            {
                // If no user is found, return HttpNotFound
                return HttpNotFound();
            }


            return View(user);
        }

        
        // POST: User/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserModel model)
        {
            try
            {
                // Retrieve the email from the session
                string userEmail = Session["Email"] as string;
                // Check if the session email is null
                if (userEmail == null)
                {
                    // Redirect to login if email is not found in the session
                    return RedirectToAction("Login", "Home");
                }


                Userrepository userRepo = new Userrepository();

                // Fetch the current user details
                var currentUser = userRepo.GetUserByEmail(userEmail);

                if (currentUser == null)
                {
                    return HttpNotFound();
                }

                
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.DateOfBirth = model.DateOfBirth;
                currentUser.Email = model.Email; 
                currentUser.Password = model.Password; 
                currentUser.ConfirmPassword = model.ConfirmPassword; 
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.State = model.State;
                currentUser.District = model.District;
                currentUser.Address = model.Address;

                // Update user in the database
                userRepo.UpdateUser(currentUser);

                // Optionally update the session email
                Session["Email"] = currentUser.Email;

                return RedirectToAction("UserPage"); 
            }
            catch (Exception ex)
            {
              
                ModelState.AddModelError("", "An error occurred while updating the profile: " + ex.Message);
                return View(model);
            }
        }


        [HttpGet]
        public ActionResult Collections()
        {
            string userEmail = Session["Email"] as string;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Home");
            }
            BookRepository bookRepo = new BookRepository();

            var books = bookRepo.GetBook(); 

            var model = new RequestModel
            {
                Books = books
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SubmitSelectedBooks(RequestModel model)
        {
            try
            {
                if (Session["Email"] == null)
                {
                    return Json(new { success = false, message = "User not authenticated." });
                }
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Submitting the book");
                    string userEmail = Session["Email"] as string;
                    BookRepository bookRepo = new BookRepository();
                    var selectedBooks = model.Books.Where(b => b.IsSelected).ToList();

                    if (selectedBooks.Count == 0)
                    {
                        return Json(new { success = false, message = "No books selected for request." });
                    }

                    foreach (var book in selectedBooks)
                    {
                        bool isSaved = bookRepo.StoreBookRequestInDatabase(book,userEmail);
                        if (!isSaved)
                        {
                            return Json(new { success = true, message = "Requested book successfully." });
                        }
                    }

                    return Json(new { success = true });
                }
                else
                {
                    // Log model state errors
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    var errorMessages = string.Join(", ", errors.Select(e => e.ErrorMessage));
                    return Json(new { success = false, message = "Invalid model state: " + errorMessages });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }


        ///request book details
        public ActionResult RequestBookDetails()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            string userEmail = Session["Email"] as string;
            BookRepository bookRepo = new BookRepository();
            ModelState.Clear();

            // Pass userEmail to RequestBook method
            var books = bookRepo.RequestBook(userEmail);

            return View(books);
        }


        //accepted book details
        public ActionResult ResponseDetails()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Home"); 
            }
            string userEmail = Session["Email"] as string;
            BookRepository repo = new BookRepository();
            ModelState.Clear();
            return View(repo.GetResponse(userEmail));
        }



        //***********user logout
        public ActionResult Logout()
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Clear authentication cookie (if using Forms Authentication)
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty)
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(authCookie);
            }

            // Redirect to the login page or home page
            return RedirectToAction("Index", "Home");
        }


    }
}
