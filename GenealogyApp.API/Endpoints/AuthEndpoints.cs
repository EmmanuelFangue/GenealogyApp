using GenealogyApp.Application.DTOs;
using GenealogyApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenealogyApp.API.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder g)
    {
        g.MapPost("/signup", async ([FromServices] IUserService users, [FromBody] RegisterUserDto dto) =>
        {
            var u = await users.RegisterAsync(dto);
            return u is null ? Results.BadRequest(new { message = "Username ou email déjà utilisé." }) : Results.Ok(u);
        });

        g.MapPost("/login", async ([FromServices] IUserService users, [FromBody] LoginDto dto) =>
        {
            var res = await users.LoginAsync(dto);
            return res is null ? Results.Unauthorized() : Results.Ok(res);
        });

        return g;
    }
}
