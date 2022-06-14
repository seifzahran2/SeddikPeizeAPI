using System;
using System.Collections.Generic;

namespace SeddikPeizeAPI.Models.ViewModels
{
    public class AuthVM
    {
        public bool Check { get; set; }
        public string Message { get; set; }
        public bool IsAuthed { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}
