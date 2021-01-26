/*
 *   Copyright (C) 218 - MVPConf Latam
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agenda.Host.Models.Configuration
{
    public class ConfigurationUser : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.FullName)
                .IsRequired(true)
                .HasMaxLength(80);

            builder.Property(p => p.Email)
                .HasMaxLength(100);

            builder.Property(p => p.Country)
                .HasMaxLength(50);

            builder.Property(p => p.State)
                .HasMaxLength(60);

            builder.Property(p => p.City)
                .HasMaxLength(100);

            builder.Property(p => p.Gender)
                .HasMaxLength(20);

            builder.Property(p => p.Phone)
                .HasMaxLength(20);

            builder.Property(p => p.Hash)
                .IsRequired(true)
                .HasMaxLength(36);

            builder.Property(p => p.IdLinkedin)
                .HasMaxLength(50);

            builder.Property(p => p.PromoCode)
                .HasMaxLength(50);

            builder.Property(p => p.Deficiency)
               .HasMaxLength(2);

            builder.Property(p => p.PaymentStatus)
               .IsRequired()
               .HasDefaultValueSql("0");

            builder.Property(p => p.Status)
              .IsRequired()
              .HasDefaultValueSql("0");

            builder.HasIndex(i => new { i.Hash, i.DocumentID });

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User);
        }
    }
}