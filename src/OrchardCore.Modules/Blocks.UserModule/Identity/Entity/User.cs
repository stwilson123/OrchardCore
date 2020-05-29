using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Users.Abstractions;

namespace BlocksCore.Users.Entity
{
    public class User :  IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ResetToken { get; set; }
        public IList<string> RoleNames { get; set; } = new List<string>();
        //public IList<UserClaim> UserClaims { get; set; } = new List<UserClaim>();
        //public IList<UserLoginInfo> LoginInfos { get; set; } = new List<UserLoginInfo>();

        public override string ToString()
        {
            return UserName;
        }
    }
}
