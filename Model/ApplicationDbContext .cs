using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace filtertion_Task.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplcationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         
            modelBuilder.Entity<Task>()
                .HasOne(t => t.TeamMember) 
                .WithMany()  
                .HasForeignKey(t => t.MemberId)
                .OnDelete(DeleteBehavior.SetNull); 

            modelBuilder.Entity<TeamMember>()
                .HasIndex(m => m.Email)
                .IsUnique();

        }
    }
}
