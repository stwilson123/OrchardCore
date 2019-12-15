namespace OrchardCore.Users.Handlers
{
    /// <summary>
    /// Represents a <see cref="UserContextBase"/> for a user creation.
    /// </summary>
    public class CreateUserContext : UserContextBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="CreateUserContext"/>.
        /// </summary>
        /// <param name="user">The <see cref="IUser"/>.</param>
        public CreateUserContext(IUser user) : base(user)
        {
        }
    }
}