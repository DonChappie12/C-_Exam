using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace c__exam.Models
{
    public class Join
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("user")]
        public int User_Id { get; set; }
        public User user { get; set; }

        [ForeignKey("activities")]
        public int Activity_Id { get; set; }
        public Activities activities { get; set; }
    }
}