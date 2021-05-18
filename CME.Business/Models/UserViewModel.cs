using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Email { get; set; }

        public int? Gender { get; set; } // 0: Nữ, 1: Nam

        public string Address { get; set; }

        public string AvatarUrl { get; set; }
    }
}
