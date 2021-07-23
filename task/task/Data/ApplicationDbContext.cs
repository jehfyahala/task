using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using task.Models;

namespace task.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //modificaciones
        public DbSet<task.Models.State> State { get; set; }
        public DbSet<task.Models.Job> Job { get; set; }
        public DbSet<task.Models.SubTask> SubTask { get; set; }
    }
}
