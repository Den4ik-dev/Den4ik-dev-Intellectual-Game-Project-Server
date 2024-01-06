using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.DTOs;

namespace server.Web.Controllers;

[ApiController]
[Route("/api/users/statistics")]
public class UserStatisticsController : ControllerBase
{
    private IUsersStatisticsService _usersStatisticsService;

    public UserStatisticsController(IUsersStatisticsService usersStatisticsService)
    {
        _usersStatisticsService = usersStatisticsService;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetFullUserStatistics()
    {
        int userId = int.Parse(User.Identity.Name);

        UserStatisticsDto userStatistics = await _usersStatisticsService.GetUserStatistics(userId);

        return Ok(userStatistics);
    }
}
