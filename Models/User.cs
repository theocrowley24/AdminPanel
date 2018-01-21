using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool verified { get; set; } = true;
        public string errorMessage { get; set; }
    }
}