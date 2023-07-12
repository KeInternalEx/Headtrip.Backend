using Headtrip.LoginServer.Models;
using Headtrip.LoginServerContext;
using Headtrip.Models.User;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Headtrip.LoginServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        private readonly IEmailSender _emailSender;
        private readonly ILogging<HeadtripLoginServerContext> _logging;

        public LoginController(
            IAuthenticationService authenticationService,
            IUserService userService,
            IAccountService accountService,
            IEmailSender emailSender,
            ILogging<HeadtripLoginServerContext> logging)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _accountService = accountService;
            _emailSender = emailSender;
            _logging = logging;
        }


        [HttpPost]
        public async Task<ActionResult> ConfirmEmail(
            [FromBody] ConfirmEmailParameters confirmEmailParameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(confirmEmailParameters.Parameter))
                    return BadRequest();

                var decryptionResult = _userService.GetUserIdFromEmailConfirmationParameter(confirmEmailParameters.Parameter);
                var result = await _userService.ConfirmEmailAndCreateAccount(decryptionResult.UserId);

                if (result.IsSuccessful && result.Account != null)
                {
                    _logging.LogInfo($"Email confirmed for UserId {decryptionResult.UserId} - AccountId {result.Account.AccountId}");

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
            }

            return StatusCode(500);
        }


        [HttpPost]
        public async Task<ActionResult> CreateUser(
            [FromBody] CreateUserParameters createUserParameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createUserParameters.Username) ||
                    string.IsNullOrWhiteSpace(createUserParameters.Password) ||
                    string.IsNullOrWhiteSpace(createUserParameters.Email))
                {
                    return BadRequest();
                }


                var result = await _userService.CreateUser(
                    createUserParameters.Username,
                    createUserParameters.Email,
                    createUserParameters.Password);

                if (result.IsSuccessful)
                {

                    var emailConfirmationUrl = $"https://{HttpContext.Request.Host}/emailconfirmation?p={result.EmailConfirmationParameter}";

                    await _emailSender.SendEmail(new EmailObject
                    {

                    });

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
            }

            return StatusCode(500);
        }



        [HttpPost]
        public async Task<ActionResult<LoginUserResult>> LoginUserByUsername(
            [FromBody] LoginUserParameters loginUserParameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginUserParameters.Username) ||
                    string.IsNullOrWhiteSpace(loginUserParameters.Password))
                {
                    return BadRequest(new LoginUserResult
                    {
                        Status = "Username and Password fields are REQUIRED"
                    });
                }

                var authenticationResult = await _authenticationService.LoginUserByUsername(loginUserParameters.Username, loginUserParameters.Password);
                if (authenticationResult.IsSuccessful)
                {
                    return Ok(new LoginUserResult
                    {
                        Token = authenticationResult.Token,
                        Expiration = authenticationResult.Expiration,
                        Status = string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
            }

            return StatusCode(500);
        }

        [HttpPost]
        public async Task<ActionResult<LoginUserResult>> LoginUserByEmail(
            [FromBody] LoginUserParameters loginUserParameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginUserParameters.Username) ||
                    string.IsNullOrWhiteSpace(loginUserParameters.Password))
                {
                    return BadRequest(new LoginUserResult
                    {
                        Status = "Username and Password fields are REQUIRED"
                    });
                }

                var authenticationResult = await _authenticationService.LoginUserByEmail(loginUserParameters.Username, loginUserParameters.Password);
                if (authenticationResult.IsSuccessful)
                {
                    return Ok(new LoginUserResult
                    {
                        Token = authenticationResult.Token,
                        Expiration = authenticationResult.Expiration,
                        Status = string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
            }

            return StatusCode(500);
        }

        [HttpPost]
        public async Task<ActionResult<LoginUserResult>> GsLoginUserByUsername(
            [FromBody] LoginUserParameters loginUserParameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginUserParameters.Username) ||
                    string.IsNullOrWhiteSpace(loginUserParameters.Password))
                {
                    return BadRequest(new LoginUserResult
                    {
                        Status = "Username and Password fields are REQUIRED"
                    });
                }

                var authenticationResult = await _authenticationService.LoginUserByUsernameForGameServer(loginUserParameters.Username, loginUserParameters.Password);
                if (authenticationResult.IsSuccessful)
                {
                    return Ok(new LoginUserResult
                    {
                        Token = authenticationResult.Token,
                        Expiration = authenticationResult.Expiration,
                        Status = string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
            }

            return StatusCode(500);
        }

        [HttpPost]
        public async Task<ActionResult<LoginUserResult>> GsLoginUserByEmail(
            [FromBody] LoginUserParameters loginUserParameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginUserParameters.Username) ||
                    string.IsNullOrWhiteSpace(loginUserParameters.Password))
                {
                    return BadRequest(new LoginUserResult
                    {
                        Status = "Username and Password fields are REQUIRED"
                    });
                }

                var authenticationResult = await _authenticationService.LoginUserByEmailForGameServer(loginUserParameters.Username, loginUserParameters.Password);
                if (authenticationResult.IsSuccessful)
                {
                    return Ok(new LoginUserResult
                    {
                        Token = authenticationResult.Token,
                        Expiration = authenticationResult.Expiration,
                        Status = string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
            }

            return StatusCode(500);
        }

        

    }
}
