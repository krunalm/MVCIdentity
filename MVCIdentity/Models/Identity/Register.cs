using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIdentity.Models.Identity
{
    public class Register
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Password must be between 6-40 characters.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage="The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Fullname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Bio { get; set; }
    }
}