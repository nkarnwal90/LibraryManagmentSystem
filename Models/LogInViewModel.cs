using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagmentSystem.Models
{
    public class LogInViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool? isloggedin { get; set; }

        public Member Member
        {
            get => default;
            set
            {
            }
        }
    }
}
