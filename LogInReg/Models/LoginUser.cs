using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogInReg.Models
{
    public class LoginUser
    {
        //[Required]
        //public string FirstName { get; set; }

        //[Required]
        //public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
