using System.ComponentModel.DataAnnotations;

namespace SSD_Lab1.Models
{
    public class Employee
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone #")]
        public long PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Incorporated Date")]
        public DateTime IncorporatedDate { get; set; }
    }
}
