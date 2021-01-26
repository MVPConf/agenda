using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Features.Authentication
{
    public partial class AuthState
    {
        public class LoginAction : IAction
        {
            public string Code { get; set; }

            public LoginAction(string code)
            {
                Code = code;
            }
        }
    }
}
