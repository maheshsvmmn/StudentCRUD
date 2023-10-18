using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Students_API.Models
{
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; internal set; }

        [Required]
        public string Name { get; set; }


        [Required]
        public string Subject { get; set; }

        public int Salary {  get; set; }


        public DateTime HiringDate {  get; internal set; }

        [Range(0 , 5)]
        public double Rating {  get; internal set; }

    }
}
