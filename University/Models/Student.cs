using System.ComponentModel.DataAnnotations;

namespace University.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30, ErrorMessage = "Max length 30")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30, ErrorMessage = "Max length 30")]
        public string LastName { get; set; }

        public Group Group { get; set; }
    }
}
