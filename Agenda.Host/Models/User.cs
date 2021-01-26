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

using System;
using System.Collections.Generic;

namespace Agenda.Host.Models
{
    public class User
    {
        public User()
        {
            DateCreated = DateTime.Now;
            DateUpdate = DateTime.Now;
            Hash = Guid.NewGuid().ToString();
            Pictures = new List<UserPicture>();

            PaymentStatus = 0;
            Country = "BR";
        }

        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string DocumentID { get; set; }
        public string Hash { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public bool Active { get; set; }
        public string IdLinkedin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdate { get; set; }
        public string PromoCode { get; set; }
        public string Deficiency { get; set; }
        public string Scholarity { get; set; }
        public int PaymentStatus { get; set; }
        public int Status { get; set; }
        public string Company { get; set; }
        public virtual IList<UserPicture> Pictures { get; set; }
        public virtual IList<Order> Orders { get; set; }

        public bool AcceptedCodeOfConduct { get; set; }
        public bool? AcceptedImageAndVideo { get; set; }
        public bool ShareInformationWithPartners { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
