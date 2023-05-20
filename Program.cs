using Newtonsoft.Json;
using System.IO;

string codigoProducto;
int codigoProducto2;
string numeroCaja;
double correlativo;
string cantidadComprada;
int cantidadComprada2;
string rutCliente;
int idVenta;
string fecha;
string fecha2;
string json;

Venta ventaBuscada;
Producto productoBuscado;
PosContext ctx = new PosContext();
VentaProducto ventaProducto;
Venta venta;

List<Venta> ventaDelDia = new List<Venta>();
List<VentaProducto> ventaProductosDia = new List<VentaProducto>();

JsonSerializerSettings settings = new JsonSerializerSettings
{
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};


DateTime fechaHoraActual = DateTime.Now;
TimeSpan horaActual = fechaHoraActual.TimeOfDay;
fecha2 = fechaHoraActual.Day.ToString() + fechaHoraActual.Month.ToString() + fechaHoraActual.Year.ToString();



Console.WriteLine("Buenos dias, ingrese el numero de caja: ");
numeroCaja = Console.ReadLine();
while(!ctx.esNumero(numeroCaja)){
Console.WriteLine("El numero ingresado no es valido, intente nuevamente.");
numeroCaja = Console.ReadLine();
}


Console.WriteLine("Indique el RUT del cliente, 888 para terminar dia de ventas");
rutCliente = Console.ReadLine();

while (rutCliente != "888"){

fechaHoraActual = DateTime.Now;
horaActual = fechaHoraActual.TimeOfDay;
fecha = fechaHoraActual.Day.ToString() + fechaHoraActual.Month.ToString() + fechaHoraActual.Hour.ToString() + fechaHoraActual.Minute.ToString() + fechaHoraActual.Second.ToString();

correlativo = double.Parse(numeroCaja + fecha);


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

json = JsonConvert.SerializeObject(ventaDelDia.ToList(), Formatting.Indented, settings);
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


