using System;
using System.Collections.Generic;

namespace NotRealCoverWeb.Models
{
    public partial class FacturaVentum
    {
        public FacturaVentum()
        {
            DetFacturaVenta = new HashSet<DetFacturaVentum>();
        }

        public int Id { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Correlativo { get; set; } = null!;
        public string Cliente { get; set; } = null!;
        public decimal TotalVenta { get; set; }

        public virtual ICollection<DetFacturaVentum> DetFacturaVenta { get; set; }
    }
}
