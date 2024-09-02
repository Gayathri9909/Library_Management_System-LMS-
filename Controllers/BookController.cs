//using library.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace library.Controllers
//{
//    public class BookController : Controller
//    {
//        // GET: Book
//        public ActionResult Book()
//        {
//            return View();
//        }
//        // GET: Book/Details/5
//        public ActionResult BookDetails()
//        {
//            BookRepository bookRepo = new BookRepository();
//            ModelState.Clear();
//            return View(bookRepo.GetBook());
//        }

//        //// GET: User/Create
//        public ActionResult BookCreate()
//        {
//            return View();
//        }

//        // POST: User/Create
//        [HttpPost]
//        public ActionResult BookCreate(BookModel Book)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    BookRepository bookRepo = new BookRepository();
//                    if (bookRepo.AddBook(Book))
//                    {
//                        ViewBag.Message="Book details added successfully";
//                    }
//                }
//                return View();
//            }
//            catch (Exception ex)
//            {
//                return View();
//            }
//        }

//        // GET: User/Edit/5
//        public ActionResult BookEdit(int BookId)
//        {
//            BookRepository bookRepo = new BookRepository();
//            return View(bookRepo.GetBook().Find(Book => Book.BookId == BookId));
//        }

//        // POST: User/Edit/5
//        [HttpPost]
//        public ActionResult BookEdit(int BookId, BookModel obj)
//        {
//            try
//            {
//                BookRepository bookRepo = new BookRepository();
//                bookRepo.UpdateBook(obj);

//                return RedirectToAction("GetBook");
//            }
//            catch (Exception ex)
//            {
//                return View();
//            }
//        }

//        // GET: User/Delete/5
//        public ActionResult DeleteBook(int BookId)
//        {
//            return View();
//        }

//        // POST: User/Delete/5
//        [HttpPost]
//        public ActionResult Delete(int BookId, BookModel obj)
//        {
//            try
//            {
//                BookRepository bookRepo = new BookRepository();
//                if (bookRepo.DeleteBook(BookId))
//                {
//                    ViewBag.AlertMsg="Book details deleted";
//                }

//                return RedirectToAction("GetBook");
//            }
//            catch (Exception ex)
//            {
//                return View();
//            }
//        }
//    }
//}