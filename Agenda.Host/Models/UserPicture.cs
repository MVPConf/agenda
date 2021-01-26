using System;
using System.Collections.Generic;
using System.Text;

namespace Agenda.Host.Models
{
    public class UserPicture
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public byte[] Picture { get; set; }
    }
}
