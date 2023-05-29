using Newtonsoft.Json;
using System.IO;

string codigoProducto;
int codigoProducto2;
string numeroCaja;
long correlativo;
string cantidadComprada;
int cantidadComprada2;
string rutCliente;
int idVenta;
string fecha;
string fecha2;
string json;
string IdSucursal = "";
string sucursalActual = "Punto encuentro";

Venta ventaBuscada;
VentaJson ventaJson; 
Producto productoBuscado;
ProductoJson productoJson;
PosContext ctx = new PosContext();
VentaProducto ventaProducto;
Venta venta;

List<VentaProductoJson> ventaProductoJsons;
List<Venta> ventaDelDia = new List<Venta>();
List<VentaJson> ventaDelDiaJson = new List<VentaJson>();

List<VentaProducto> ventaProductosDia = new List<VentaProducto>();

JsonSerializerSettings settings = new JsonSerializerSettings
{
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};


DateTime fechaHoraActual = DateTime.Now;
TimeSpan horaActual = fechaHoraActual.TimeOfDay;
fecha2 = fechaHoraActual.Day.ToString() + fechaHoraActual.Month.ToString() + fechaHoraActual.Year.ToString();


string archivoIntentario = @"/Users/dragoperic/Desktop/archivosPOS/inventario/inventario.json";

string inventario = File.ReadAllText(archivoIntentario);


List<ProductoInvenario> productosActualizados = JsonConvert.DeserializeObject<List<ProductoInvenario>>(inventario);

//Actualizacion de inventario

foreach (var productoActualizado in productosActualizados){
    Console.WriteLine("Codigo desde inventerio.json: "+ productoActualizado.Codigo);


        Producto productoExistente = ctx.Productos.FirstOrDefault(p => p.Codigo == productoActualizado.Codigo);
        if ((productoExistente != null) && (sucursalActual == productoActualizado.NombreSucursal))
        {
            Console.WriteLine("Codigo encontrado: "+  productoExistente.Codigo);
            Console.WriteLine("producto de la sucursal: "+ productoActualizado.NombreSucursal);
            productoExistente.Codigo = productoActualizado.Codigo;
            productoExistente.Nombre = productoActualizado.Nombre;
            productoExistente.Descripcion = productoActualizado.Descripcion;
            productoExistente.Precio = productoActualizado.Precio;
            IdSucursal = productoActualizado.IdSucursal.ToString();
            
        }
        if((productoExistente == null) && (sucursalActual == productoActualizado.NombreSucursal)){
            Producto nuevoProducto = new Producto{
            Codigo = productoActualizado.Codigo,
            Nombre = productoActualizado.Nombre,
            Descripcion = productoActualizado.Descripcion,
            Precio = productoActualizado.Precio,
            };
                ctx.Productos.Add(nuevoProducto);
                ctx.SaveChanges();
        }
        
        
}

ctx.SaveChanges();


Console.WriteLine("Buenos dias, ingrese el numero de caja: ");
numeroCaja = Console.ReadLine();
while(!ctx.esNumero(numeroCaja)){
Console.WriteLine("El numero ingresado no es valido, intente nuevamente.");
numeroCaja = Console.ReadLine();
}


Console.WriteLine("Indique el RUT del cliente, 888 para terminar dia de ventas");
rutCliente = Console.ReadLine();

while (rutCliente != "888"){

ventaProductoJsons = new List<VentaProductoJson>();

fechaHoraActual = DateTime.Now;
horaActual = fechaHoraActual.TimeOfDay;
fecha = fechaHoraActual.Day.ToString() + fechaHoraActual.Month.ToString() + fechaHoraActual.Hour.ToString() + fechaHoraActual.Minute.ToString() + fechaHoraActual.Second.ToString();

correlativo = long.Parse(numeroCaja + fecha);


Console.WriteLine("Ingrese el numero del producto, para finalizar ingrese 999");
codigoProducto = Console.ReadLine();
while(!ctx.esNumero(codigoProducto)){
Console.WriteLine("El numero ingresado no es valido, intente nuevamente.");
codigoProducto = Console.ReadLine();
}
codigoProducto2 = int.Parse(codigoProducto);


productoBuscado = ctx.Productos.Where(p => p.Codigo == codigoProducto2).FirstOrDefault();



while (productoBuscado == null){
    Console.WriteLine("No existe ese producto dentro de la base de datos");
    Console.WriteLine("Intentelo nuevamente: ");

    codigoProducto = Console.ReadLine();
    while(!ctx.esNumero(codigoProducto)){
        Console.WriteLine("El numero ingresado no es valido, intente nuevamente. 999 para finalizar:");
        codigoProducto = Console.ReadLine();
    }
    codigoProducto2 = int.Parse(codigoProducto);
    if(codigoProducto2 == 999){
        break;
    }
    productoBuscado = ctx.Productos.Where(p => p.Codigo == codigoProducto2).FirstOrDefault();
}
    if(codigoProducto2 != 999){
        Console.WriteLine("Producto: " + productoBuscado.Nombre.ToUpper()+" , Precio: $"+productoBuscado.Precio);
        productoJson = new ProductoJson(){Idproducto = productoBuscado.Idproducto, Nombre = productoBuscado.Nombre, Precio = productoBuscado.Precio, Codigo = productoBuscado.Codigo, Descripcion = productoBuscado.Descripcion};


    }   


venta = new Venta(){Correlativo = correlativo, Fecha = fechaHoraActual.Date, Hora = fechaHoraActual.TimeOfDay, RutCliente = rutCliente};
ctx.Ventas.Add(venta);
ventaDelDia.Add(venta);


ctx.SaveChanges();

ventaBuscada = ctx.Ventas.OrderByDescending(e => e.Idventa).FirstOrDefault();

idVenta = ventaBuscada.Idventa;

while(codigoProducto2 != 999){

Console.Write("Indique la cantidad a comprar: ");
cantidadComprada = Console.ReadLine();
while(!ctx.esNumero(cantidadComprada)){
    Console.WriteLine("El numero ingresado no es valido, intente nuevamente.");
    cantidadComprada = Console.ReadLine();
}
cantidadComprada2 = int.Parse(cantidadComprada);

ventaProducto =  new VentaProducto(){Idproducto = productoBuscado.Idproducto ,Idventa = idVenta , Cantidad = cantidadComprada2 ,Precio = productoBuscado.Precio}; 
ctx.VentaProductos.Add(ventaProducto);
ventaProductosDia.Add(ventaProducto);
ventaProductoJsons.Add(new VentaProductoJson(){Idproducto = productoBuscado.Idproducto ,Idventa = idVenta , Cantidad = cantidadComprada2 ,Precio = productoBuscado.Precio});



Console.WriteLine("Ingrese el numero del producto, para finalizar ingrese 999");
codigoProducto = Console.ReadLine();
while(!ctx.esNumero(codigoProducto)){
    Console.WriteLine("El numero ingresado no es valido, intente nuevamente.");
    codigoProducto = Console.ReadLine();
}
codigoProducto2 = int.Parse(codigoProducto);


if(codigoProducto2== 999){
    break;
}

productoBuscado = ctx.Productos.Where(p => p.Codigo == codigoProducto2).FirstOrDefault();

while (productoBuscado == null){
    Console.WriteLine("No existe ese producto dentro de la base de datos");
    Console.WriteLine("Intentelo nuevamente: ");

    codigoProducto = Console.ReadLine();
    while(!ctx.esNumero(codigoProducto)){
        Console.WriteLine("El numero ingresado no es valido, intente nuevamente. 999 para finalizar:");
        codigoProducto = Console.ReadLine();
    }
    codigoProducto2 = int.Parse(codigoProducto);
    if(codigoProducto2 == 999){
        break;
    }
    productoBuscado = ctx.Productos.Where(p => p.Codigo == codigoProducto2).FirstOrDefault();
}
    if(codigoProducto2 != 999){
        Console.WriteLine("Producto: " + productoBuscado.Nombre.ToUpper()+" , Precio: $"+productoBuscado.Precio);

    }   

}
ventaJson = new VentaJson(){IdSucursal = IdSucursal ,Idventa = idVenta, Correlativo = correlativo, Fecha = fechaHoraActual.Date, Hora = fechaHoraActual.TimeOfDay, RutCliente = rutCliente};
ventaJson.VentaProducto = ventaProductoJsons;
ventaDelDiaJson.Add(ventaJson);

ctx.SaveChanges();




int total = 0;
Console.WriteLine("----------------------------------------------------------------");
 Console.WriteLine("CORRELATIVO: "+ correlativo);
 Console.WriteLine("Productos: ");
 foreach (var b in ctx.VentaProductos.ToList()){
    if (b.Idventa == ventaBuscada.Idventa){
    foreach (var c in ctx.Productos.ToList()){
        if(c.Idproducto == b.Idproducto){
            Console.WriteLine("-->" + c.Nombre.ToUpper() +" x"+b.Cantidad.ToString());
            total += (c.Precio*b.Cantidad);
            }
        }
    }
 }
 Console.WriteLine("Total a pagar: $"+ total);
 Console.WriteLine("----------------------------------------------------------------");


Console.WriteLine("Indique el RUT del cliente, 888 para terminar dia de ventas");
rutCliente = Console.ReadLine();

}


//generacion de archivos.

string directorio;
directorio = @"/Users/dragoperic/Desktop/archivosPOS/ventas";

if (!Directory.Exists(directorio))
{
    Directory.CreateDirectory(directorio);
}


json = JsonConvert.SerializeObject(ventaDelDiaJson.ToList(), Formatting.Indented, settings);
File.WriteAllText(directorio+"/ventasDia_"+fecha2+".json", json);

json = JsonConvert.SerializeObject(ctx.Ventas.ToList(), Formatting.Indented, settings);
File.WriteAllText(directorio+"/ventasGeneral.json", json);


directorio = @"/Users/dragoperic/Desktop/archivosPOS/ventasProductos";

if (!Directory.Exists(directorio))
{
    Directory.CreateDirectory(directorio);
}


json = JsonConvert.SerializeObject(ventaDelDia.ToList(), Formatting.Indented, settings);
File.WriteAllText(directorio+"/ventasProductoDia_"+fecha2+".json", json);


json = JsonConvert.SerializeObject(ctx.VentaProductos.ToList(), Formatting.Indented, settings);
File.WriteAllText(directorio+"/ventasProductoGeneral.json", json);


