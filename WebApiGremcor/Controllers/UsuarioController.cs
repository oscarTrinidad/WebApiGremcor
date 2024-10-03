using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using WebApiGremcor.Model;

namespace WebApiGremcor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : Controller
    {
        Usuario usuarioModel = new Usuario();
        private readonly IConfiguration configuration;
        public UsuarioController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Usuario pUsuario)
        {
            ResponseModel usuarios = new ResponseModel();
            usuarios = await usuarioModel.InicioSesion(pUsuario);
            if (usuarios.status)
            {
                if (usuarios.data == null)
                {
                    ResponseModel response = new ResponseModel();
                    response.status = false;
                    response.message = "Usuario y/o Contraseña incorrecto!";
                    return Ok(response);
                }
                else
                {
                    return await BuildToken(usuarios.data);
                }
            }
            else
            {
                return Ok(usuarios);
            }
        }

        [HttpPost("lista")]
        public async Task<IActionResult> ListaUsuario([FromBody] Usuario pUsuario)
        {
            ResponseModel response = new ResponseModel();
            response.status = false;
            try
            {
                var aUsuarios = await usuarioModel.ListaUsuario(pUsuario);

                response.data = new
                {
                    aUsuarios,
                };
                response.status = true;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost("mantenimiento")]
        public async Task<IActionResult> MantenimientoUsuario([FromBody] Usuario pUsuario)
        {
            ResponseModel response = new ResponseModel();
            response.status = false;
            try
            {
                var aUsuario = await usuarioModel.MantenimientoUsuario(pUsuario);

                if (aUsuario.status)
                {
                    response.data = new
                    {
                        aUsuario = aUsuario.data,
                    };
                    response.status = true;
                }
                else
                {
                    response = aUsuario;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }

        private async Task<IActionResult> BuildToken(Usuario usuario)
        {
            var expiration = DateTime.UtcNow.AddYears(1);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.sUsuario),
                new Claim("Id", usuario.iIdUsuario.ToString()),
                new Claim("Usuario", usuario.sUsuario),
                new Claim("Password", usuario.sContrasenia),
                new Claim("Expiration", expiration.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            ResponseModel response = new ResponseModel();

            response.status = true;

            response.data = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration,
                usuario
            };

            return Ok(response);
            //    new
            //{
            //    token = new JwtSecurityTokenHandler().WriteToken(token),
            //    expiration = expiration,
            //    usuario = usuario
            //}
        }
    }
}
