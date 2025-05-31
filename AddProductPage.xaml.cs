using ClaseNetMaui.Models; // <-- ¡Asegúrate de tener este using!
using System.Collections.Generic;

namespace ClaseNetMaui.Views;

public partial class AddProductPage : ContentPage, IQueryAttributable
{
    Producto? product = null;
    Inventario inventario = new Inventario();

    public AddProductPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await inventario.CargarProductosDesdeArchivoAsync();

        if (product != null)
        {
            LTitle.Text = "Modificar Producto";
            EName.Text = product.Nombre;
            EdNote.Text = product.Descripcion;
            ECate.Text = product.Categoria?.Nombre;
            // Si tienes campos para cantidad y precio, agrégalos aquí
            // ECantidad.Text = product.Cantidad.ToString();
            // EPrecio.Text = product.Precio.ToString("F2");
        }
        else
        {
            LTitle.Text = "Agregar Producto";
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        product = null;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("product") && query["product"] is Producto _product)
        {
            product = _product;
        }
    }

    private async void BtnSave_Clicked(object sender, EventArgs e)
    {
        int cantidad = 0;
        float precio = 0.0f;
        Dictionary<string, string> propiedadesEspecificas = new();

        // Si tienes campos para cantidad y precio, descomenta y adapta:
        // int.TryParse(ECantidad.Text, out cantidad);
        // float.TryParse(EPrecio.Text, out precio);

        if (string.IsNullOrEmpty(EName.Text) || string.IsNullOrEmpty(EdNote.Text))
        {
            string msg = string.IsNullOrEmpty(EName.Text) ? $"- Nombre del Producto. {Environment.NewLine}" : "";
            msg += string.IsNullOrEmpty(EdNote.Text) ? $"- Nota del Producto. {Environment.NewLine}" : "";
            await DisplayAlert("Advertencia", $"Falta llenar los campos: {Environment.NewLine} {msg}", "Ok");
        }
        else
        {
            bool respuesta = await DisplayAlert("Guardar Datos", "¿Estás seguro de guardar los datos?", "Sí", "No");

            if (respuesta)
            {
                // Usa SIEMPRE ClaseNetMaui.Models.Categoria aquí
                var categoria = new ClaseNetMaui.Models.Categoria(ECate.Text);

                var nuevoProducto = new Producto()
                {
                    Nombre = EName.Text,
                    Descripcion = EdNote.Text,
                    Cantidad = cantidad,
                    Precio = precio,
                    Categoria = categoria, // ¡Siempre asigna un objeto!
                    PropiedadesEspecificas = new SortedDictionary<string, string>()
                };

                // Si es edición, reemplaza el producto existente
                if (product != null)
                {
                    var existente = inventario.productos.FirstOrDefault(p => p.Nombre == product.Nombre);
                    if (existente != null)
                    {
                        existente.Nombre = nuevoProducto.Nombre;
                        existente.Descripcion = nuevoProducto.Descripcion;
                        existente.Cantidad = nuevoProducto.Cantidad;
                        existente.Precio = nuevoProducto.Precio;
                        existente.Categoria = nuevoProducto.Categoria;
                        existente.PropiedadesEspecificas = nuevoProducto.PropiedadesEspecificas;
                    }
                }
                else // Si es nuevo, lo agrega
                {
                    inventario.productos.Add(nuevoProducto);
                }

                await inventario.GuardarProductosEnArchivoAsync();

                Dictionary<string, object> parametros = new();
                parametros["product"] = nuevoProducto;
                AppShell.Parameters = parametros;
                AppShell.Current.SendBackButtonPressed();
            }
        }
    }

    private async void BtnDelete_Clicked(object sender, EventArgs e)
    {
        if (product == null)
            return;

        bool respuesta = await DisplayAlert("Eliminar Productos", "¿Estás seguro de eliminar este producto?", "Si", "No");

        if (respuesta)
        {
            inventario.productos.RemoveAll(p => p.Nombre == product.Nombre);
            await inventario.GuardarProductosEnArchivoAsync();

            await DisplayAlert("Eliminar Producto", "Producto Eliminado", "Ok");
            Dictionary<string, object> parametros = new();
            parametros["product"] = product;
            AppShell.Parameters = parametros;
            AppShell.Current.SendBackButtonPressed();
        }
    }
}