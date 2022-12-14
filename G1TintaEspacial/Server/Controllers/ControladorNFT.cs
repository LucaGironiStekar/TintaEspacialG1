using G1TintaEspacial.BD.Data.Entidades;
using G1TintaEspacial.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace G1TintaEspacial.Server.Controllers
{
    [ApiController]
    [Route("api/NFT")]
    public class ControladorNFT : ControllerBase
    {
        private readonly dbcontex contex;
        public ControladorNFT(dbcontex contex)
        {
            this.contex = contex;
        }
        [HttpGet]
        public async Task<ActionResult<List<NFT>>> Get()
        {
            var resp = await contex.NFTs.ToListAsync();
            return resp;
        }
        #region GET ID
        [HttpGet("id:int")]
        public async Task<ActionResult<NFT>> Get(int id)
        {
            var NFT = await contex.NFTs.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (NFT == null)
            {
                return NotFound($"No existe el NFT con dicha ID={id}");
            }
            return NFT;
        }
        #endregion



        [HttpPost]
        public async Task<ActionResult<int>> Post(NFT nft)
        {
            try
            {
                contex.NFTs.Add(nft);
                await contex.SaveChangesAsync();
                return nft.Id;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] NFT nft)
        {
            if (id != nft.Id)
            {
                return BadRequest("Datos incorrectos");
            }

            var mati = contex.NFTs.Where(e => e.Id == id).FirstOrDefault();
            if (mati == null)
            {
                return NotFound("No existe el NFT a modificar");
            }

            mati.Id = nft.Id;
            mati.NombreObra = nft.NombreObra;
            mati.Descripcion = nft.Descripcion;
            mati.Autor = nft.Autor;
            mati.Precio = nft.Precio;
            mati.Token = nft.Token;
            mati.ImagenNFT = nft.ImagenNFT;
            try
            {
                //throw(new Exception("Cualquier Verdura"));
                contex.NFTs.Update(mati);
                contex.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Los datos no han sido actualizados por: {e.Message}");
            }
        }
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var mati = contex.NFTs.Where(x => x.Id == id).FirstOrDefault();

            if (mati == null)
            {
                return NotFound($"El registro {id} no fue encontrado");
            }
            try
            {
                contex.NFTs.Remove(mati);
                contex.SaveChanges();
                return Ok($"El registro de {mati.Token} ha sido borrado.");
            }
            catch (Exception e)
            {
                return BadRequest($"Los datos no pudieron eliminarse por: {e.Message}");
            }
        }

    }

}


