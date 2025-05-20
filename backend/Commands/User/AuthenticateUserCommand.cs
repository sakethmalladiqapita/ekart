/// <summary>
/// CQRS Command.
/// Represents a request to authenticate a user using their email and password.
/// </summary>
public class AuthenticateUserCommand
{
    /// <summary>
    /// The email address entered by the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The plain password entered by the user (should be hashed internally).
    /// </summary>
    public string Password { get; set; }
}
