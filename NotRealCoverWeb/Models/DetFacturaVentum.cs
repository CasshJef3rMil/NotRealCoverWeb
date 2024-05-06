using System;
using System.Collections.Generic;

namespace NotRealCoverWeb.Models
{
    public partial class DetFacturaVentum
    {
        

        public int Id { get; set; }
        public int IdFacturaVenta { get; set; }
        public string Album { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public virtual FacturaVentum IdFacturaVentaNavigation { get; set; } = null!;
    }
}
