/// <summary>
/// CQRS Command.
/// Represents a request to create a new user account during registration.
/// </summary>
public class CreateUserCommand
{
    /// <summary>
    /// The email address provided during registration.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The raw or hashed password for the new user (depending on service implementation).
    /// </summary>
    public string Password { get; set; }
}
