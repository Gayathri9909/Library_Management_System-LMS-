using library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;

namespace library.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin page
        public ActionResult Adminhome()
        {
            return View();
        }

        /*-----------User section-------------*/
        public ActionResult Usermanagement()
        {
           
            Userrepository userRepo = new Userrepository();
            ModelState.Clear();
            return View(userRepo.GetUser());
        }
       
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usermanagement/Create
        [HttpPost]
        public ActionResult Create(UserModel User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Userrepository userRepo = new Userrepository();

                    if (userRepo.AddUser(User))
                    {
                        ViewBag.Message="User details added successfully";
                    }
                    else
                    {
                        ViewBag.Message="Failed to proceed";

                    }
                    return RedirectToAction("Usermanagement");

                }
              
                
            
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

       // to delete user details

        [HttpPost]
        public ActionResult Delete(string email)
        {
            try
            {
                Userrepository userRepo = new Userrepository();
                bool success = userRepo.DeleteUser(email);

                if (success)
                {
                    ViewBag.AlertMsg = "User details deleted";
                }
                else
                {
                    ViewBag.AlertMsg = "Failed to delete user details";
                }

                return RedirectToAction("Usermanagement");
            }
            catch (Exception ex)
            {
                // Log exception here
                ViewBag.AlertMsg = "An error occurred while trying to delete the user";
                return View("Error");
            }
        }







        /*------------book section-------------*/
        ///
        public ActionResult Bookmanagement()
        {
            BookRepository bookRepo = new BookRepository();
            ModelState.Clear();
            return View(bookRepo.GetBook());
        }
        // GET: Bookmanagement/BookCreate
        public ActionResult BookCreate()
        {
            return View();
        }

        // POST: Bookmanagement/BookCreate
        [HttpPost]
      
        public ActionResult BookCreate(BookModel book, HttpPostedFileBase bookImage)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (ModelState.IsValid)
                    {
                        BookRepository bookRepo = new BookRepository();

                        if (bookRepo.AddBook(book))
                        {
                            ViewBag.Message="Book details added successfully";
                        }
                        return RedirectToAction("Bookmanagement");
                    }

                }

                return View(book);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while adding the book.";
                return View(book);
            }
        }


        // GET: Bookmanagement/BookEdit/5
        public ActionResult BookEdit(int id)
        {

            BookRepository bookRepo = new BookRepository();
            return View(bookRepo.GetBook().Find(Book => Book.BookId == id));
        }

        // POST: Bookmanagement/BookEdit/5
        [HttpPost]
        public ActionResult BookEdit(int id, BookModel obj)
        {
            try
            {
                BookRepository bookRepo = new BookRepository();
                bookRepo.UpdateBook(obj);
                return View(bookRepo.GetBook());
                //return RedirectToAction("GetBook");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the book.");
                return View();
            }
        }

       

        // POST: Admin/BookDelete/5
        [HttpPost]
        public ActionResult BookDelete(int id)
        {
            try
            {
                BookRepository bookRepo = new BookRepository();
                if (bookRepo.DeleteBook(id))
                {
                    ViewBag.AlertMsg = "Book details deleted";
                }
                else
                {
                    ViewBag.AlertMsg = "Failed to delete book details";
                }

                return RedirectToAction("Bookmanagement");
            }
            catch (Exception ex)
            {
                // Log the exception
                ViewBag.AlertMsg = "An error occurred while trying to delete the book";
                return View("Error"); // Ensure you have an Error view to handle exceptions
            }
        }



        /************contact section**************/

        public ActionResult ContactMessage()
        {
            ContactRepository contactRepo = new ContactRepository();
            ModelState.Clear();
            return View(contactRepo.GetContact());
        }

        /*************admin section********************/

        // GET: Admin/AdminDetails
        public ActionResult AdminDetails()
        {
            AdminRepository adminRepo = new AdminRepository();
            ModelState.Clear();
            return View(adminRepo.GetAdmin());
        }

        //// GET: Admin/AdminCreate
        public ActionResult AdminCreate()
        {
            return View();
        }

        // POST: Admin/AdminCreate
        [HttpPost]
        public ActionResult AdminCreate(AdminModel admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AdminRepository adminRepo = new AdminRepository();
                    if (adminRepo.AddAdmin(admin))
                    {
                        ViewBag.Message="Admin details added successfully";
                    }
                }
                return RedirectToAction("AdminDetails");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Admin/EditAdmin/5
        public ActionResult EditAdmin(int id)
        {
            AdminRepository adminRepo = new AdminRepository();
            return View(adminRepo.GetAdmin().Find(Admin => Admin.AdminId == id));
        }

        // POST: Admin/EditAdmin/5
        [HttpPost]
        public ActionResult EditAdmin(int  id, AdminModel obj)
        {
            try
            {
                AdminRepository adminRepo = new AdminRepository();
                adminRepo.UpdateAdmin(obj);

                return RedirectToAction("AdminDetails");
            }
            catch (Exception ex)
            {
                return View();
            }
        }


       //admin delete

        [HttpPost]
        public ActionResult AdminDelete(int id)
        {
            try
            {
                AdminRepository adminRepo = new AdminRepository();

                // Attempt to delete the admin
                bool success = adminRepo.DeleteAdmin(id);

                if (success)
                {
                    ViewBag.AlertMsg = "Admin details deleted successfully.";
                }
                else
                {
                    ViewBag.AlertMsg = "Failed to delete admin details.";
                }

               
                return RedirectToAction("AdminDetails");
            }
            catch (Exception ex)
            {
                
                ViewBag.AlertMsg = "An error occurred while trying to delete the admin.";
               
                return View("Error"); 
            }
        }

     

        //**manage requests

        public ActionResult ManageRequests()
        {

            BookRepository bookRepo = new BookRepository();

            var books = bookRepo.RequestBooks();

            var model = new ResponseModel
            {
                AcceptBooks = books,
              
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SubmitResponseBooks(ResponseModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookRepository bookRepo = new BookRepository();
                    var selectedBooks = model.AcceptBooks.Where(b => b.IsSelected).ToList();

                    if (selectedBooks.Count == 0)
                    {
                        return Json(new { success = false, message = "No books selected for response." });
                    }

                    foreach (var book in selectedBooks)
                    {
                        var requestStatus = model.IsAcceptRequest ? "Accepted" : "Denied";
                        bool isSaved = bookRepo.StoreBookResponseInDatabase(book, book.UserEmail, requestStatus);
                        if (!isSaved)
                        {
                            return Json(new { success = true, message = "Book response is successfully sent." });
                        }
                    }

                    return Json(new { success = true, message = "Response is successfully sent.", updatedBooks = selectedBooks });
                }
                return Json(new { success = false, message = "Invalid model state." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }




        /**********************logout section*****************/

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
