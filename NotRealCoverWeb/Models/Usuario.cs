using System;
using System.Collections.Generic;

namespace NotRealCoverWeb.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Apellido { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte Estatus { get; set; }
        public string Rol { get; set; } = null!;
    }
}
