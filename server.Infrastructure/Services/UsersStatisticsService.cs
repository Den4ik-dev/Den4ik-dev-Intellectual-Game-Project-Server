using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.Database;
using server.Domain.DTOs;

namespace server.Infrastructure.Services;

public class UsersStatisticsService : IUsersStatisticsService
{
    private ApplicationContext _db;

    public UsersStatisticsService(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<UserStatisticsDto> GetUserStatistics(int userId)
    {
        var userStatistics = new UserStatisticsDto()
        {
            CountOfUserQuestions = await _db.UserQuestions.CountAsync(uq => uq.UserId == userId),
            CountOfCorrectUserQuestions = await _db.UserQuestions.CountAsync(
                uq => uq.UserId == userId && uq.Complete
            )
        };

        return userStatistics;
    }
}
