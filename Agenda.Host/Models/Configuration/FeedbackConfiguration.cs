using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Host.Models.Configuration
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedbacks");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedAt)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(p => p.SpeakerEvaluationScore)
                .IsRequired();
            builder.Property(p => p.SpeechEvaluationScore)
                .IsRequired();
            builder.Property(p => p.SpeechId)
                .IsRequired();
            builder.Property(p => p.Notes)
                .HasMaxLength(2000);
        }
    }
}
