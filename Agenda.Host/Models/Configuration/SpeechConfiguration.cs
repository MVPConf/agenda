using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Agenda.Host.Models.Configuration
{
    public class SpeechConfiguration : IEntityTypeConfiguration<Speech>
    {
        public void Configure(EntityTypeBuilder<Speech> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.ExternalId)
                .IsRequired();
            builder.Property(p => p.Title)
                .IsRequired();
            builder.Property(p => p.Description)
                .IsRequired();
            builder.Property(p => p.Track)
                .IsRequired();
            builder.Property(p => p.JoinUrl)
                .IsRequired();
            builder.Property(p => p.Speakers)
                .IsRequired()
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<List<string>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
