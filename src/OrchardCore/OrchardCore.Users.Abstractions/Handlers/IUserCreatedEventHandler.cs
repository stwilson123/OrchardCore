using System.Threading.Tasks;

namespace OrchardCore.Users.Handlers
{
    /// <summary>
    /// Contract for user creation event
    /// </summary>
    public interface IUserCreatedEventHandler
    {
        /// <summary>
        /// Occurs when the user is created.
        /// </summary>
        /// <param name="context">The <see cref="CreateUserContext"/>.</param>
        Task CreatedAsync(CreateUserContext context);
    }
}