using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace c__exam.Models
{
    public class Activities
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Must Provide a Title")]
        [MinLength(2, ErrorMessage="Must be 2 characters long or more")]
        public string Title { get; set; }

        [Required(ErrorMessage="Must provide a Time")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get ; set; }

        [Required(ErrorMessage="Must Provide a Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage="Must provide Duratino of activity")]
        public double Duration { get; set; }

        [Required(ErrorMessage="Must provide Description")]
        [MinLength(10, ErrorMessage="Must be 10 characters long or more")]
        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("user")]
        public int User_Id { get; set; }
        public User user { get; set; }

        public List<Join> Joining { get; set; }

        public Activities()
        {
            Joining = new List<Join>();
        }
    }
}