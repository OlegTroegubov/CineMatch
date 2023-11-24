using CineMatch.Application.Common.Exceptions.Auth;
using CineMatch.Application.Common.Exceptions.User;
using CineMatch.Application.Features.Token;
using CineMatch.Application.Features.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineMatch.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("likeMovie")]
    public async Task<IActionResult> LikeMovie(LikeMovieCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _mediator.Send(command, cancellationToken));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _mediator.Send(command, cancellationToken));
        }
        catch (InvalidTokenException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (ExpiredTokenException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (TokenException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Registration(RegistrationUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return Created("", await _mediator.Send(command, cancellationToken));
        }
        catch (ExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokensDto>> Login(LoginUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _mediator.Send(command, cancellationToken));
        }
        catch (AuthException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _mediator.Send(new LogoutUserCommand(), cancellationToken));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch
        {
            return Unauthorized();
        }
    }
}