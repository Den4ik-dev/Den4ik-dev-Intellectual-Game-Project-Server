using server.Domain.DTOs;

namespace server.Application.Interfaces;

public interface IUsersStatisticsService
{
    public Task<UserStatisticsDto> GetUserStatistics(int userId);
}
