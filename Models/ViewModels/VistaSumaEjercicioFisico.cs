using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Proyecto1_2024.Models.ViewModels

{
    public class VistaSumaEjercicioFisico
    {
        public string? TipoEjercicioNombre { get; set; }
        public int TotalidadMinutos { get; set; }
        public int TotalidadDiasConEjercicio { get; set; }
        public int TotalidadDiasSinEjercicio { get; set; }
        public List<VistaEjercicioFisico>? DiasEjercicios { get; set; }
    }
}


