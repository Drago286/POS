using System;
using System.Collections.Generic;


public partial class VentaProducto
{
    public int IdventaProducto { get; set; }

    public int? Idproducto { get; set; }

    public int? Idventa { get; set; }

    public int Cantidad { get; set; }

    public int Precio { get; set; }

    public virtual Producto? IdproductoNavigation { get; set; }

    public virtual Venta? IdventaNavigation { get; set; }

    //public VentaProducto objVentaProducto { get; set; }
}
