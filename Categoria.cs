using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseNetMaui.Models
{
    public class Categoria
    {
        public string Nombre { get; set; }
        public List<string> Propiedades { get; private set; }
        public List<Categoria> Subcategorias { get; private set; }

        public Categoria(string nombre)
        {
            Nombre = nombre;
            Propiedades = new List<string>();
            Subcategorias = new List<Categoria>();
        }

        public void AgregarPropiedad(string propiedad)
        {
            Propiedades.Add(propiedad);
        }

        public void AgregarSubcategoria(Categoria subcategoria)
        {
            Subcategorias.Add(subcategoria);
        }
    }
}
