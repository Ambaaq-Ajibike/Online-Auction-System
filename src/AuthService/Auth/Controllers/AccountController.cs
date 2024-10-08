﻿using Auth.DTO;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController(UserManager<ApplicationUser> userManager, IConfiguration config) : ControllerBase
	{
		[HttpPost("register")]
		public async Task< IActionResult> Register(RegisterUserDTO userDTO)
		{
			if(ModelState.IsValid)
			{
				ApplicationUser AppUser = new ApplicationUser()
				{
					UserName = userDTO.UserName,
					Email = userDTO.Email,
					PasswordHash = userDTO.Password,
				};
			 IdentityResult Result=	await userManager.CreateAsync(AppUser,userDTO.Password);
				if (Result.Succeeded)
				{
					//this when you make to add registered user as an admin
					await userManager.AddToRoleAsync(AppUser, "Admin");
					return Ok("Account Created");
				}
				return BadRequest(Result.Errors);
			}
			return BadRequest(ModelState);
		}

		[HttpPost("login")]
		public async Task< IActionResult> Login(LoginUserDTO userDTO)
		{
			if(ModelState.IsValid)
			{
				ApplicationUser? UserFromDB= await userManager.FindByNameAsync(userDTO.UserName);
				if (UserFromDB != null)
				{
					bool found= await userManager.CheckPasswordAsync(UserFromDB,userDTO.Password);
					if (found)
					{
						//Create Token
						List<Claim> myclaims =
                        [
                            new Claim(ClaimTypes.Name, UserFromDB.UserName),
                            new Claim(ClaimTypes.Email, UserFromDB.Email),
                            new Claim(ClaimTypes.NameIdentifier, UserFromDB.Id),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        ];

						var roles=await userManager.GetRolesAsync(UserFromDB);
						foreach (var role in roles)
						{
							myclaims.Add(new Claim(ClaimTypes.Role, role));
						}

						var SignKey = new SymmetricSecurityKey(
						   Encoding.UTF8.GetBytes(config["JWT:SecritKey"]));

						SigningCredentials signingCredentials =
							new SigningCredentials(SignKey, SecurityAlgorithms.HmacSha256);

						JwtSecurityToken mytoken = new JwtSecurityToken(
						   issuer: config["JWT:ValidIss"],//provider create token
						   audience: config["JWT:ValidAud"],//consumer url
						expires: DateTime.Now.AddHours(1),
						   claims: myclaims,
						   signingCredentials: signingCredentials);
						return Ok(new
						{
							token = new JwtSecurityTokenHandler().WriteToken(mytoken),
							expired = mytoken.ValidTo
						});
					}
				}
				return BadRequest("Invalid Request");
			}
			return BadRequest(ModelState);
		}

	}
}
