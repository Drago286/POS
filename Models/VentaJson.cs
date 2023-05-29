using System;
using System.Collections.Generic;


public partial class VentaJson
{
    public int Idventa { get; set; }

    public string IdSucursal { get; set; }

    public long Correlativo { get; set; }

    public DateTime? Fecha { get; set; }

    public TimeSpan? Hora { get; set; }

    public string? RutCliente { get; set; }

     public List<VentaProductoJson> VentaProducto { get; set; }
}
