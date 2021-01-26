/*
 *Copyright(C) 218 - MVPConf Latam
*
* This program is free software: you can redistribute it and/or modify
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
    public class ConfigurationTicket : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(t => t.TicketKey);
            builder.Property(t => t.Price);
            builder.Property(t => t.Description).HasMaxLength(200);
            builder.Property(t => t.Enable);
            builder.Ignore(t => t.OrderId);
            
            builder.HasOne(t => t.Order)
                   .WithOne(o => o.Ticket)
                   .HasForeignKey<Ticket>(p => p.TicketKey);
        }
    }
}
