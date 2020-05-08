namespace BlocksCore.Abstractions.Security
{
    public interface IUserContext
    {
        IUserIdentifier GetCurrentUser();
    }
}