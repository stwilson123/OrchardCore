using System;

namespace BlocksCore.Users.Abstractions
{
    public interface IUser
    {
        string Id { get; set; }
        string UserName { get; }

    }
}
