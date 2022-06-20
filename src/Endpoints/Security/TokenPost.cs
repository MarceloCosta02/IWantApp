﻿using IWantApp.Domain.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IWantApp.Endpoints.Security;

public static class TokenPost
{
    public static string Template => "/token";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(LoginRequest loginRequest, IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);

        if (user == null)
            return Results.BadRequest();

        if (await userManager.CheckPasswordAsync(user, loginRequest.Password))
            return Results.BadRequest();

        var claims = await userManager.GetClaimsAsync(user);
        var subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, loginRequest.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        });

        subject.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddHours(3)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Results.Ok(new { token = tokenHandler.WriteToken(token) });
    }
}
