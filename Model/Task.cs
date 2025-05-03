using System.ComponentModel.DataAnnotations;

namespace filtertion_Task.Model
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? MemberId { get; set; }

        public virtual TeamMember? TeamMember { get; set; }
    }
}
