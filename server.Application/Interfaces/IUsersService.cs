using System.Linq.Expressions;
using server.Domain.DTOs;
using server.Domain.Models;

namespace server.Application.Interfaces;

public interface IUsersService
{
    public Task<User?> GetUserAsync(int userId);
    public Task<User?> GetUserAsync(string loginUser);
    public Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate);
    public Task SetRefreshTokenAsync(User user, string refreshToken);
    public Task<User> AddUserAsync(RegisteredUserDto loginUser);
}
