using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace library.Models
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName IS Reqired")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "Password IS Reqired")]

        public string Password { get; set; }

        [Required(ErrorMessage = "IsActivate Filed IS Reqired")]

        public bool IsActivate { get; set; }

        [Required(ErrorMessage = "Role IS Reqired")]

        public string Role { get; set; }

    }
}