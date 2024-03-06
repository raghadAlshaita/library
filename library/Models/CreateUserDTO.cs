using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "UserName Is Reqierd")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Is Reqierd")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsActivate { get; set; }
        public bool IsAdmin{ get; set; }

    }
}