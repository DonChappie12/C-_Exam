using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace c__exam.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public List<Join> Joining { get; set; }
        public List<Activities> Activity { get; set; }

        public User()
        {
            Activity = new List<Activities>();
            Joining = new List<Join>();
        }
    }
}