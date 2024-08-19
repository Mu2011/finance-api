using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager) : ControllerBase
{
  private readonly UserManager<AppUser> _userManger = userManager;
  private readonly ITokenService _tokenService = tokenService;
  private readonly SignInManager<AppUser> _signInManager = signInManager;

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginDto loginDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var user = await _userManger.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

    if (user is null) return Unauthorized("Invalid username!");

    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

    if (!result.Succeeded) return Unauthorized("Username not found and/or password in correct");
    return Ok
    (
      new NewUserDto
      {
        UserName = user.UserName,
        Email = user.Email,
        Token = _tokenService.CreateToken(user)
      }
    );
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
  {
    try
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var appUser = new AppUser
      {
        UserName = registerDto.UserName,
        Email = registerDto.Email
      };

      var createdUser = await _userManger.CreateAsync(appUser, registerDto.Password);
      if (createdUser.Succeeded)
      {
        var roleResult = await _userManger.AddToRoleAsync(appUser, "User");
        if (roleResult.Succeeded)
          return Ok
          (
            new NewUserDto
            {
              UserName = appUser.UserName,
              Email = appUser.Email,
              Token = _tokenService.CreateToken(appUser)
            }
          );
        else
          return StatusCode(500, roleResult.Errors);
      }
      else
        return StatusCode(500, createdUser.Errors);

    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }
}