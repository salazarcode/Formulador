using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Data.Repositorios;

namespace Formulador.Dominio
{
    public class Formula
    {
        public DetalleRepository _DetalleRepository;
        public FormulaRepository _FormulaRepository;

        public Formula()
        {
            _DetalleRepository = new DetalleRepository();
            _FormulaRepository = new FormulaRepository();
        }

        public int IdFormula { get; set; }
        public string Articulo { get; set; }
        public string CodArticulo { get; set; }
        public string CodTemporal { get; set; }
        public string Nombre { get; set; }
        public string IdCliente { get; set; }
        public int IdTipoProducto { get; set; }
        public string CodDiv { get; set; }
        public string CodFam { get; set; }
        public string CodSubFam { get; set; }
        public int IdUndProd { get; set; }
        public int IdKgBolsa { get; set; }
        public int IdConcentracion { get; set; }
        public int IdEspecie { get; set; }
        public bool EsOficial { get; set; }
        public string Observaciones { get; set; }
        public float FactorRendimiento { get; set; }
        public float TotalInsumos { get; set; }
        public float Dosis { get; set; }
        public float RendimientoNeto { get; set; }
        public float CantDosis { get; set; }
        public float DosisBolsa { get; set; }
        public float Costo { get; set; }
        public float CantAlimento { get; set; }
        public string OnservacionesProd { get; set; }
        public bool BloqueadoEdicion { get; set; }
        public DateTime FechaBloqueo { get; set; }
        public string UsuarioBloqueo { get; set; }
        public int IdTipoImportacion { get; set; }
        public bool ConCostoCeroEnDetalle { get; set; }
        public bool EsEmpaque { get; set; }
        public bool EsActivo { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioActualizacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool IsRegExpirationPeriod { get; set; }
        public async Task<List<Detalle>> Details()
        {
            var res = await _DetalleRepository.GetByFormula(this.IdFormula);
            return res;
        }
        public static async Task<List<Formula>> All(string user)
        {
            var _FormulaRepository = new FormulaRepository();
            var res = await _FormulaRepository.All(user);
            return res;
        }
    }
}
