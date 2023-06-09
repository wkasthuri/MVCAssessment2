
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MVCAssessment2.Models
{
    public class CSIROContext : IdentityDbContext
    {
        public DbSet<Applicant> applicant { get; set; }

        public DbSet<Courses> courses { get; set; }

        public DbSet<Universities> universities { get; set; }

        public DbSet<AspNetUsers> aspNetUsers { get; set; }

        public DbSet<GPA> gpa { get; set; }

        public CSIROContext(DbContextOptions<CSIROContext> options): base(options)
        {
            Database.EnsureCreated();
        } 
    }
}
