using G1TintaEspacial.Server.Registro;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace G1TintaEspacial.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public  static Usuario usuario = new Usuario();
        private readonly IConfiguration _configuration;

        //private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            this._configuration = configuration;
            //_configuration = configuration;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<Usuario>>Registro(UsuarioDTO request)
        {
            CrearContraseñaHash(request.Contraseña, out byte[] contraseñaHash, out byte[] contraseñaSalt);
            usuario.NombreUsuario = request.NombreUsuario;
            usuario.ContraseñaHash = contraseñaHash;
            usuario.ContraseñaSalt = contraseñaSalt;
            //CONTRASENA HASH:
            return Ok(usuario);

        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UsuarioDTO request)
        {
            if (usuario.NombreUsuario != request.NombreUsuario)
            {
                return BadRequest("No se encuentra el usuario");

            }
            if (!VerificaContraseñaHash(request.Contraseña, usuario.ContraseñaHash, usuario.ContraseñaSalt))
            {
                return BadRequest("Contraseña incorrecta.");
            }

            //string token = CreateToken(usuario);//aca tambien tira error SWAGGER

            //return Ok("AMIS Y ARYA");//forma parte de la prueba
            return Ok();
        }



        #region Metodos
        //private string CreateToken(Usuario usuario)//intente de string a int
        //{
        //    List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, usuario.NombreUsuario)
        //    };

        //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds);

        //    string jwt = new JwtSecurityTokenHandler().WriteToken(token);//aca tira error el swagger
        //    return jwt;

        //    //return string.Empty; ;
        //}


        


        private void CrearContraseñaHash(string contraseña, out byte[] contraseñaHash, out byte[] contraseñaSalt)
        {
            using (var hmac = new HMACSHA512())//instancia criptográfica
            {
                contraseñaSalt = hmac.Key;
                contraseñaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(contraseña));

            }

        }

        private bool VerificaContraseñaHash (string contraseña, byte[] contraseñaHash, byte[] contraseñaSalt)
        {
            using (var hmac = new HMACSHA512(contraseñaSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(contraseña));
                return computedHash.SequenceEqual(contraseñaHash);

            }
        }


        #endregion
    }
}
