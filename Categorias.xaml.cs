using System.Collections.ObjectModel;
using System.Reflection;

namespace ClaseNetMaui.Views
{
    public partial class Categoria : ContentPage
    {
        public Categoria()
        {
            InitializeComponent();
        }
        private async void LVCat_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is ClaseNetMaui.Models.Categoria categoriaSeleccionada)
            {
                await DisplayAlert("Categoría Seleccionada", $"Has seleccionado la categoría: {categoriaSeleccionada.Nombre}", "Ok");
            }
        }
    }
}
