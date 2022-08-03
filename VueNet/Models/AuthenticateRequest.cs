using System;
using System.ComponentModel.DataAnnotations;

namespace VueNet.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string? Account { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}

