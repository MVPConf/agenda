using Agenda.Host.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Agenda.Host.Models
{
    public class AgendaDbContext : DbContext
    {
        public AgendaDbContext([NotNull] DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgendaDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine);

        public DbSet<Speech> Speeches { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Winner> Winners { get; set; }
    }
}
