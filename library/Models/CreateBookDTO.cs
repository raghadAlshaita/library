using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class CreateBookDTO
    {
        [Required(ErrorMessage = "Title IS Reqired")]
        public string Title { get; set; }

        [Required(ErrorMessage = "ISBN IS Reqired")]

        public string ISBN { get; set; }

        [Required(ErrorMessage = "Author IS Reqired")]
        public string Author { get; set; }
        public int Status { get; set; }
    }
}