using System.ComponentModel.DataAnnotations;

namespace Employewebapp.Models
{
    public class CreateEmployee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        [Display(Name="Enter Email")]

        public string Description { get; set; }
        [Required(ErrorMessage = "please select the department.")]
        public Dept? Department  { get; set; }    
    
        public string Phone { get; set; }
        [Range(1000 , 10000)]
        public int Salary { get; set; }

        
    }
}
