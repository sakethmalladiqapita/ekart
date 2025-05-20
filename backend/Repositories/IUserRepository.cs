using ekart.Models;

/// <summary>
/// DDD Repository Interface for the User aggregate.
/// Provides abstraction over persistence logic for users.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieve a user by their unique identifier.
    /// </summary>
    Task<User> GetByIdAsync(string id);

    /// <summary>
    /// Retrieve a user by their email address (e.g., for login).
    /// </summary>
    Task<User> GetByEmailAsync(string email);

    /// <summary>
    /// Create and persist a new user.
    /// </summary>
    Task CreateAsync(User user);

    /// <summary>
    /// Update an existing user's data (e.g., cart, address, password).
    /// </summary>
    Task UpdateAsync(User user);
}
