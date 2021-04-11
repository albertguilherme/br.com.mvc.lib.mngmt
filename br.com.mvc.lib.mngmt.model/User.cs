using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace br.com.mvc.lib.mngmt.model
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }

    }

    public enum Roles
    {
        ADMIN, 
        USER,
        CLIENT
    }
}
