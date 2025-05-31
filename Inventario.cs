using System.Text.Json;
using System.Collections.Generic;
using System.Reflection;
using ClaseNetMaui.Models;

public class Inventario
{
    public List<Producto> productos { get; private set; }
    public List<Categoria> categorias { get; private set; }

    public Inventario()
    {
        productos = new List<Producto>();
        categorias = new List<Categoria>();

        // 🔹 Cargar los productos desde el recurso embebido
        CargarProductosDesdeRecurso();
    }

    private void CargarProductosDesdeRecurso()
    {
        try
        {
            // 🔹 Obtener el ensamblado actual
            var assembly = typeof(Inventario).GetTypeInfo().Assembly;

            // 🔹 Leer el contenido de `Productos.json` embebido en los recursos
            var resourceName = "ClaseNetMaui.Resources.Raw.Productos.json";
            using var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName));
            string json = reader.ReadToEnd();

            // 🔹 Convertir el JSON en una lista de productos
            productos = JsonSerializer.Deserialize<List<Producto>>(json) ?? new List<Producto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar productos desde recurso embebido: {ex.Message}");
        }
    }

    public void EliminarProducto(string nombre)
    {
        productos.RemoveAll(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
    }

    public Categoria BuscarCategoria(string nombre)
    {
        return categorias.FirstOrDefault(c => c.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
    }

    public List<Producto> ListarProductosPorCategoria(string nombreCategoria)
    {
        return productos.Where(p => p.Categoria.Nombre.Equals(nombreCategoria, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public Producto BuscarProducto(string nombre)
    {
        return productos.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
    }

    public void ActualizarStock(string nombre, int nuevaCantidad)
    {
        Producto producto = BuscarProducto(nombre);
        if (producto != null)
        {
            producto.Cantidad = nuevaCantidad;
        }
    }

    public void AgregarProductoConPropiedades(string nombre, string descripcion, int cantidad, float precio, string nombreCategoria, Dictionary<string, string> propiedades)
    {
        Categoria categoria = BuscarCategoria(nombreCategoria) ?? new Categoria(nombreCategoria);

        Producto nuevoProducto = new Producto(nombre, descripcion, cantidad, precio, categoria)
        {
            PropiedadesEspecificas = new SortedDictionary<string, string>(propiedades)
        };

        productos.Add(nuevoProducto);
    }

    public bool EditarProducto(string nombre, string nuevaDescripcion, int nuevaCantidad, float nuevoPrecio, string nuevaCategoria, Dictionary<string, string> nuevasPropiedades)
    {
        Producto producto = BuscarProducto(nombre);
        if (producto != null)
        {
            producto.Descripcion = nuevaDescripcion;
            producto.Cantidad = nuevaCantidad;
            producto.Precio = nuevoPrecio;

            if (!producto.Categoria.Nombre.Equals(nuevaCategoria, StringComparison.OrdinalIgnoreCase))
            {
                producto.Categoria = BuscarCategoria(nuevaCategoria) ?? new Categoria(nuevaCategoria);
            }

            producto.PropiedadesEspecificas.Clear();
            foreach (var propiedad in nuevasPropiedades)
            {
                producto.PropiedadesEspecificas[propiedad.Key] = propiedad.Value;
            }

            return true;
        }

        return false;
    }

    public void AgregarCategoria(string nombreCategoria)
    {
        if (!categorias.Any(c => c.Nombre.Equals(nombreCategoria, StringComparison.OrdinalIgnoreCase)))
        {
            categorias.Add(new Categoria(nombreCategoria));
        }
    }
}
