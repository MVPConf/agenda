using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Host.Models.Configuration
{
    public class WinnerConfiguration : IEntityTypeConfiguration<Winner>
    {
        public void Configure(EntityTypeBuilder<Winner> builder)
        {
            builder.ToTable("Winners");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.UserHash)
                .IsRequired();
            builder.Property(p => p.CreatedAt)
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}
