using System.ComponentModel.DataAnnotations;

namespace University.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(25, ErrorMessage = "Max length 25")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "Max length 50")]
        public string Description { get; set; }
        public Course Course { get; set; }
    }
}
