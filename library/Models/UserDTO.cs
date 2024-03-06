using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName Is Reqierd")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Is Reqierd")]
        [DataType(DataType.Password)]   
        public string Password { get; set; }
        [Required(ErrorMessage = "IsActivate Is Reqierd")]

        public bool IsActivate { get; set; }

        [Required(ErrorMessage = "Role Is Reqierd")]


        //TO DO Get From DB 
        public string Role { get; set; }
    }
}