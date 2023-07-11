using Headtrip.LoginServer.Models;
using Headtrip.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Headtrip.LoginServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly 

        public LoginController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(CreateUserModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userManager.CreateAsync(
                new IdentityUser() { UserName = userModel.UserName, Email = userModel.Email },
                userModel.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);


            // TODO: Need to send confirmation email

            return Ok();
        }

        [HttpPost("BearerToken")]
        public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest)

        // TODO: Need to create jwt authentication method
        // TODO: That method needs to generate a session ID for the game server
        // TODO: That session is valid until the player logs out
        // TODO: If a player doesn't log out (possible if disconnected0, session gets closed after 5 minutes of inactivity
        // TOOD: If a player logs back in, and a session already exists for them, assign them that session

    }
}
