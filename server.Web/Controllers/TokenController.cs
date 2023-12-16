using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.DTOs;
using server.Domain.Models;
using server.Web.ResponseModels;
using System.Security.Claims;

namespace server.Web.Controllers;

[ApiController, Route("/api/tokens")]
public class TokenController : ControllerBase
{
  [HttpPost("refresh")]
  public async Task<IActionResult> RefreshToken([FromBody] TokenDto token, 
    [FromServices] IValidator<TokenDto> tokenValidator,
    [FromServices] ITokenService tokenService,
    [FromServices] IUsersService usersService)
  {
    FluentValidation.Results.ValidationResult tokenValidatorResult = tokenValidator.Validate(token);
    if (!tokenValidatorResult.IsValid)
      return BadRequest(new Response(tokenValidatorResult.Errors.First().ErrorMessage));

    int userId;
    ClaimsPrincipal? principal;

    try
    {
      principal = tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
      userId = int.Parse(principal.Identity.Name);
    }
    catch (Exception ex)
    {
      return BadRequest(new Response("Некорректные данные", ex.Message));
    }

    User? user = await usersService.GetUserAsync(userId);

    if(user is null || 
       user.RefreshToken != token.RefreshToken || 
       user.RefreshTokenExpiryTime <= DateTime.Now)
    {
      return BadRequest(new Response("Время жизни refresh token истекло"));
    }

    TokenDto responseToken = new TokenDto();
    
    responseToken.AccessToken = tokenService.GenerateAccessToken(principal.Claims);
    responseToken.RefreshToken = tokenService.GenerateRefreshToken();

    await usersService.SetRefreshTokenAsync(user, responseToken.RefreshToken);

    return Ok(responseToken);
  }
}
