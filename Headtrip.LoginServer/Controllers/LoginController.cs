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

        // TODO: Need to create jwt authentication method
        // TODO: That method needs to generate a session ID for the game server
        // TODO: That session is valid until the player logs out
    }
}
