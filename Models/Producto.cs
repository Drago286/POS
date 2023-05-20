using System;
using System.Collections.Generic;


public partial class Producto
{
    public int Idproducto { get; set; }

    public int Codigo { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int Precio { get; set; }

    public virtual ICollection<VentaProducto> VentaProductos { get; set; } = new List<VentaProducto>();

}
