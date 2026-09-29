using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Auth;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokens;

    public AuthController(JwtTokenService tokens)
    {
        _tokens = tokens;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest body)
    {
        DemoUser? user = DemoUsers.Find(body.UserName, body.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new { accessToken = _tokens.CreateAccessToken(user), role = user.Role });
    }
}
