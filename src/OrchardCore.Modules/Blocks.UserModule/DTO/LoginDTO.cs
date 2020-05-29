using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Users.Abstractions;

namespace Blocks.UserModule.DTO
{
    public class LoginDTO : IUser
    {
       // [Required]
        public string UserName { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string Id { get; set; }
    }
}
