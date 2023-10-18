using API_Task_System_V6.Models;
using API_Task_System_V6.Token;
using Domain.Interfaces;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Task_System_V6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _iUser;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;


        public UserController(IUser iUser, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _iUser = iUser;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("api/CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                return Unauthorized();
            }

            var result = await _iUser.ExistUser(login.Email, login.Password);

            if (result)
            {
                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("Empresa - Projeto DDD")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("UsuarioAPINumero", "1") 
                .AddExpiry(5)
                .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("api/AddUser")]
        public async Task<IActionResult> AddUser([FromBody] Login login)
        {
            if(string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                return Ok("Falta alguns dados");
            }

            await _iUser.AddUser(login.Email, login.Password, login.Age, login.Telephone);
            return Ok("Usuário adicionado com sucesso");
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("api/CreateTokenIdentity")]
        public async Task<IActionResult> CreateTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                return Unauthorized();
            }

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {

                var idUser = await _iUser.ReturnUserId(login.Email);

                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("Empresa - Projeto DDD")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("idUser", idUser)
                .AddExpiry(5)
                .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("api/AddUserIdentity")]
        public async Task<IActionResult> AddUserIdentity([FromBody] Login login)    
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                return Ok("Falta alguns dados");
            }

            var userIdentity = new ApplicationUser
            {
                UserName = login.Email,
                Email = login.Email,
                Telephone = login.Telephone,
                Usertype = Usertype.Common
            };

            var result = await _userManager.CreateAsync(userIdentity, login.Password);

            if (result.Errors.Any())
            {
                return Ok(result.Errors);
            }

            var userId = await _userManager.GetUserIdAsync(userIdentity);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email 
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado2 = await _userManager.ConfirmEmailAsync(userIdentity, code);

            if (resultado2.Succeeded)
                return Ok("Usuário Adicionado com sucesso");
            else
                return Ok("Erro ao confirmar usuário");

        }

    }
}