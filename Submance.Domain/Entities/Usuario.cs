namespace Submance.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // ⚠️ CAMBIO: En tu BD es un texto ("Admin", "Staff"), no un número.
        public string Rol { get; set; } = "Usuario";

        // Mantenemos bool aquí, pero en el Repo lo convertimos desde "Activo/Inactivo"
        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}