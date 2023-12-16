using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.Database;
using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace server.Infrastructure.Services;
public class UsersService : IUsersService
{
  private ApplicationContext _db;
  public UsersService(ApplicationContext db)
  {
    _db = db;
  }

  public async Task<User?> GetUserAsync(int userId) =>
    await _db.Users.FindAsync(userId);

  public async Task<User?> GetUserAsync(string loginUser) =>
    await _db.Users.FirstOrDefaultAsync(u => u.Login == loginUser);

  public async Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate) =>
    await _db.Users.FirstOrDefaultAsync(predicate);

  public async Task SetRefreshTokenAsync(User user, string refreshToken)
  {
    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(15);
    user.RefreshToken = refreshToken;

    await _db.SaveChangesAsync();
  }

  public async Task<User> AddUserAsync(RegisteredUserDto loginUser)
  {
    User user = new User()
    {
      Login = loginUser.Login,
      Password = Convert.ToHexString(
        SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(loginUser.Password))),
      RoleId = 1 /* user role */
    };

    _db.Users.Add(user);
    
    await _db.SaveChangesAsync();

    return user;
  }
}
