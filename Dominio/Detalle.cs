using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Data.Repositorios;

namespace Formulador.Dominio
{
    public class Detalle
    {
        public int IdFormulaDet { get; set; }
        public int IdFormula { get; set; }
        public int Orden { get; set; }
        public string Articulo { get; set; }
        public float Dosis { get; set; }
        public float Cantidad { get; set; }
        public float PrecioOrig { get; set; }
        public float PrecioConDscto { get; set; }
        public float PorcDsctoPrec { get; set; }
        public float Costo { get; set; }
        public float Porcentaje { get; set; }
        public float Concentracion { get; set; }
        public string UnidMed { get; set; }
        public bool RequiereDosis { get; set; }
        public bool IncluirEnSumTotCant { get; set; }
        public bool EsDobleFuente { get; set; }
        public int IdDobleFuente { get; set; }
        public string CodDobleFuente { get; set; }
        public bool EsDelete { get; set; }
        public bool EsRegistEnGranelYEmpaq { get; set; }
    }
}
