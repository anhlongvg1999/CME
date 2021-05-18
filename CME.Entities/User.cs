using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Entities
{
    [Table("auth_Users")]
    public class User : BaseTable<User>
    {
        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Firstname { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Email { get; set; }

        public int? Gender { get; set; } // 0: Nữ, 1: Nam

        public string Address { get; set; }

        public string AvatarUrl { get; set; }

    }
}
