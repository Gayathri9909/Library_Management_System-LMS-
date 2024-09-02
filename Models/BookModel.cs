using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace library.Models
{
    public class BookModel
    {
        [Required]
        [Display(Name = "Book ID")]
        public int BookId { get; set; }

        [StringLength(255, ErrorMessage = "Book title cannot exceed 255 characters.")]
        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }

     
        [StringLength(255, ErrorMessage = "Author name cannot exceed 255 characters.")]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Number of books must be at least 1.")]
        [Display(Name = "Number of Copies")]
        public int NumberOfBooks { get; set; }

      
        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
        [Display(Name = "Category")]
        public string Category { get; set; }

      
        [StringLength(13, MinimumLength = 5, ErrorMessage = "ISBN must be between 10 and 13 characters.")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

      
        [Range(1500, 2024, ErrorMessage = "Publish Year must be a valid year.")]
        [Display(Name = "Publish Year")]
        public int PublishYear { get; set; }

        [Display(Name = "User Email")]
        public string UserEmail { get; set; }
      

        public bool IsSelected { get; set; }
        public string Status { get; set; }
    }

    public class RequestModel
    {
        public string UserEmail {  get; set; }
           
        public List<BookModel> Books { get; set; }
       
        
    }

    public class ResponseModel
    {
        public List<BookModel> AcceptBooks { get; set; }
        public string UserEmail { get; set; }
        public bool IsAcceptRequest { get; set; }
    }
}
