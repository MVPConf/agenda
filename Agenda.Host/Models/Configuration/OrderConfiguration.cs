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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.OrderId);
            builder.Property(o => o.UserId);
            builder.Property(o => o.MerchantOrderId);
            builder.Property(o => o.MerchantId);
            builder.Property(o => o.TicketKey);
            builder.Property(o => o.PromoCode);
            builder.Property(o => o.DateTime);
            builder.Property(o => o.Price).HasColumnType("money");

            builder.HasOne(o => o.Ticket)
                   .WithOne(t => t.Order)
                   .HasForeignKey<Order>(p => p.TicketKey);

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders);
        }
    }
}
