using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace filtertion_Task.Model
{
    public class TeamMember
    {
        [Key]
        public int MemberId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

    }
}
