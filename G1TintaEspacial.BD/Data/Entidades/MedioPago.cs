using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace G1TintaEspacial.BD.Data.Entidades
{
    [Index(nameof(Alias), Name = "MedioPagoAlias_UQ", IsUnique = true)]
    public class MedioPago : Herencia
    {
       
        [Required(ErrorMessage = "El telefono es obligatorio.")]
        public string Alias { get; set; }


        //public List<Usuario> Usuarios { get; set; }


    }

}
    

