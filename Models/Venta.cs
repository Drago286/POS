using System;
using System.Collections.Generic;


public partial class Venta
{
    public int Idventa { get; set; }

    public double Correlativo { get; set; }

    public DateTime? Fecha { get; set; }

    public TimeSpan? Hora { get; set; }

    public string? RutCliente { get; set; }

   // public Venta objVenta { get; set; }

    public virtual ICollection<VentaProducto> VentaProductos { get; set; } = new List<VentaProducto>();
}
