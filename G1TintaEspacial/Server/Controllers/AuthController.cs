using G1TintaEspacial.Data;
using G1TintaEspacial.Server.Registro;
using G1TintaEspecial.Data.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace G1TintaEspacial.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //controler de registro
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly dbcontex _context;

        //private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, dbcontex context)
        {
            this._configuration = configuration;
            this._context = context;
            //_configuration = configuration;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<string>> Registro(UsuarioDTO request)
        {

            var (contraseñaHash, contraseñaSalt) = CrearContraseñaHash(request.Contraseña);

            this._context.Usuarios.Add(new Usuario
            {
                NombreUsuario = request.NombreUsuario,
                ContraseñaHash = contraseñaHash,
                ContraseñaSalt = contraseñaSalt,
                Email = "asd",
            });// error debido a que faltan campos requirdos por completar

            await this._context.SaveChangesAsync();

            ////CONTRASENA HASH:
            return Ok("Usuario creado correctamente");

        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(UsuarioDTO request)
        {

            Usuario? UserBD = await this._context.Usuarios.FirstOrDefaultAsync(Usuario => Usuario.NombreUsuario == request.NombreUsuario);

            if (UserBD == null)
            {
                return BadRequest("No se encuentra el usuario");
            }

            if (!this.VerificaContraseñaHash(request.Contraseña, UserBD.ContraseñaHash, UserBD.ContraseñaSalt))
            {
                return BadRequest("Contraseña incorrecta");
            }
            var Result = new
            {
                Token = CrearToken(UserBD), //Creamos un nuevo metodo y obtiene usuario.
                User = new
                {
                    Email = UserBD.Email,
                    Id = UserBD.Id,
                    NombreUsuario = UserBD.NombreUsuario.ToString(),
                },
            };

            return Ok(Result);

            //string token = CreateToken(usuario);//aca tambien tira error SWAGGER

            //return Ok("AMIS Y ARYA");//forma parte de la prueba
            //return Ok();
        }



        #region Metodos

        private (byte[], byte[]) CrearContraseñaHash(string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;

            using (var hmac = new HMACSHA512()) //Algoritmo de firma 
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                passwordSalt = hmac.Key;
            }

            return (passwordHash, passwordSalt);
        }

        private string CrearToken(Usuario user)
        {
            //Permisos para describir la informacion del usuario
            List<Claim> claims = new List<Claim>
            {
               new Claim(ClaimTypes.Email, user.Email) //Permiso de seguridad
            };

            //Clave simetrica
            var key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                 this._configuration.GetSection("AppSettings:Token").Value // Obtenemos dato de appSettings para crear Token
                )
            );


            //Definimos la configuracion del token 
            var Credencial = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);//Credenciales de firma 

            //Defino la carga del token 
            JwtSecurityToken token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.Now.AddHours(2),
               signingCredentials: Credencial
            );

            //Defino la cadena de token que quiero que retorne 
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private bool VerificaContraseñaHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        #endregion
    }
}
