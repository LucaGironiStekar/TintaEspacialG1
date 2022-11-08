namespace G1TintaEspacial.Server.Registro
{
    public class Usuario
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public byte[] ContraseñaHash { get; set; }
        public byte[] ContraseñaSalt { get; set; }

    }
}
