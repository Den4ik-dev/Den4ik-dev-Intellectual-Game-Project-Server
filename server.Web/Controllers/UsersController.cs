﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.DTOs;
using server.Domain.Models;
using server.Web.ResponseModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace server.Web.Controllers;
[ApiController, Route("/api/users")]
public class UsersController : ControllerBase
{
  private IUsersService _usersService;
  public UsersController(IUsersService usersService)
  {
    _usersService = usersService;
  }

  [HttpPost("reg")]
  public async Task<IActionResult> Register([FromBody] RegisteredUserDto loginUser,
    [FromServices] IValidator<RegisteredUserDto> registeredUserValidator)
  {
    ValidationResult regiteredUserValidatorResult = 
      registeredUserValidator.Validate(loginUser);

    if (!regiteredUserValidatorResult.IsValid)
      return BadRequest(new Response(regiteredUserValidatorResult.Errors.First().ErrorMessage));

    if (await _usersService.GetUserAsync(loginUser.Login) != null)
      return BadRequest(new Response("Пользователь с данным логино уже существует"));

    await _usersService.AddUserAsync(loginUser);

    return Ok(new Response("Пользователь успешно зарегестрирован!"));
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser,
    [FromServices] IValidator<LoginUserDto> loginUserValidator,
    [FromServices] ITokenService tokenService)
  {
    ValidationResult loginUserValidatorResult = 
      loginUserValidator.Validate(loginUser);

    if (!loginUserValidatorResult.IsValid)
      return BadRequest(new Response(loginUserValidatorResult.Errors.First().ErrorMessage));

    User? user = await _usersService.GetUserAsync(u => 
      u.Login == loginUser.Login &&
      u.Password == Convert.ToHexString(
        SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(loginUser.Password))
      ));

    if (user == null)
      return NoContent(); /* status code 204 */

    TokenDto token = new TokenDto()
    {
      AccessToken = tokenService.GenerateAccessToken(
        new[] { 
          new Claim(ClaimTypes.Name, user.Id.ToString()),
          new Claim(ClaimTypes.NameIdentifier, user.Login),
          new Claim(ClaimTypes.Role, user.Role.Name)
        }),
      RefreshToken = tokenService.GenerateRefreshToken()
    };
    
    await _usersService.SetRefreshTokenAsync(user, token.RefreshToken);

    return Ok(token);
  }

  [HttpGet, Authorize]
  public async Task<IActionResult> GetUserInfo()
  {
    int userId = int.Parse(User.Identity.Name);

    UserDto user = new UserDto()
    {
      Login = (await _usersService.GetUserAsync(userId))?.Login
    };

    return Ok(user);
  }
}