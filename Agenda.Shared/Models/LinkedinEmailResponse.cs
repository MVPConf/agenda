using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class LinkedinEmailResponse
    {
        [JsonPropertyName("elements")]
        public List<Element> Elements { get; set; }
    }

    public class Element
    {
        [JsonPropertyName("handle")]
        public string Handle { get; set; }
        [JsonPropertyName("handle~")]
        public UserEmail EmailHandle { get; set; }
    }

    public class UserEmail
    {
        [JsonPropertyName("emailAddress")]
        public string Email { get; set; }
    }
}
