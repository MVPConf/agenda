using BlazorState;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Agenda.Features.Authentication
{
    public partial class AuthState : State<AuthState>
    {
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Role { get; private set; }
        public string Hash { get; set; }
        public bool LoggedIn { get; set; }
        public bool IsLoading { get; set; }
        public string ErrorMessage { get; set; }
        public string StreamUser { get; set; }
        public string StreamPass { get; set; }

        public override void Initialize()
        {
            IsLoading = false;
            LoggedIn = false;
            Name = string.Empty;
            Email = string.Empty;
            Hash = string.Empty;
            ErrorMessage = string.Empty;
            Role = string.Empty;
            StreamUser = string.Empty;
            StreamPass = string.Empty;
        }
    }
}
