using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using telefone_book_api.Models.Dto;
using telefone_book_api.Services;

namespace telefone_book_api.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]

public class TokenController : ControllerBase
{
    private readonly UserManager<TelephoneBookUser> _userManager;
    private readonly ITokenService _tokenService;

    private const string _provider = "identity";

    public TokenController(UserManager<TelephoneBookUser> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> GetExternalToken([FromBody] ExternalUserInfo userInfo)
    {
        TelephoneBookUser? user = await _userManager.FindByLoginAsync(_provider, userInfo.ProviderKey);

        if(user!=null)
        {
            return Ok(new AuthToken(_tokenService.GenerateToken(user.UserName!)));
        }
        return NotFound();
    }
     
    [HttpPost("register/user")]
    public async Task<IActionResult> GetToken([FromBody] ExternalUserInfo userInfo)
    {
        TelephoneBookUser? user = await _userManager.FindByLoginAsync(_provider, userInfo.ProviderKey);

        var result = IdentityResult.Success;

        if (user is null)
        {
            user = new TelephoneBookUser()
            {
                UserName = userInfo.Username
            };
            result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user,
                    new UserLoginInfo(_provider, userInfo.ProviderKey, displayName: null));
            }
        }

        if (result.Succeeded)
        {
            return Ok(new AuthToken(_tokenService.GenerateToken(user.UserName!)));
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        return ValidationProblem(ModelState);
    }
}
