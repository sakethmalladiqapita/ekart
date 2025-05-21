using MediatR;
using ekart.Models;

public class CreateUserCommand : IRequest<User>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
