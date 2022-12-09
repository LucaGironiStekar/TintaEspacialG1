using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace G1TintaEspacial.BD.Data.Entidades
{
    //[Index(nameof(Id), Name = "NFT_UQ", IsUnique = true)]
    public class NFT : Herencia//: Usuario 
    {

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string NombreObra { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string Autor { get; set; }
        
        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string ImagenNFT { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public int Precio { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string MercadoPago { get; set; }

        //agregar relacion de nft con usuarios

        public int UsuarioId { get; set; } //clave foranea de conecta nft con usuario


    }
}
