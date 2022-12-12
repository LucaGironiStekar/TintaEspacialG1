using G1TintaEspacial.BD.Data.Entidades;
using G1TintaEspacial.Data;
using G1TintaEspecial.Data.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace G1TintaEspacial.Server.Controllers
{
    [ApiController]
    [Route("api/Registro")]
    public class Registro : ControllerBase
    {
        private readonly dbcontex contex;
        public Registro(dbcontex contex)
        {
            this.contex = contex;
        }

        #region GET ID
        [HttpGet("id:int")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            var usuario = await contex.Usuarios.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound($"No existe el Usuario con dicha ID={id}");
            }
            return usuario;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<int>> Post(Usuario usuario)
        {
            try
            {
                contex.Usuarios.Add(usuario);
                await contex.SaveChangesAsync();
                return usuario.Id;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest("Error al cargar los datos");
            }
            var mati2 = contex.Usuarios.Where(e => e.Id == id).FirstOrDefault();
            if (mati2 == null)
            {
                return NotFound("No existe el registro a modificar");
            }
            mati2.NombreUsuario = usuario.NombreUsuario;
            mati2.Email = usuario.Email;
            mati2.Contraseña = usuario.Contraseña;
            mati2.ImagePerfil = usuario.ImagePerfil;

            try
            {
                contex.Usuarios.Update(mati2);
                contex.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"el registro no han sido actualizados por: {e.Message}");
            }
        }
        #endregion

        #region DELETE
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var mati2 = contex.Usuarios.Where(x => x.Id == id).FirstOrDefault();

            if (mati2 == null)
            {
                return NotFound($"El registro {id} no fue encontrado");
            }
            try
            {
                contex.Usuarios.Remove(mati2);
                contex.SaveChanges();
                return Ok($"El registro de {mati2.Id} ha sido borrado.");
            }
            catch (Exception e)
            {
                return BadRequest($"Los datos no pudieron eliminarse por: {e.Message}");
            }
        }
        #endregion
    }
}