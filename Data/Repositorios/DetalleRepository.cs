using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Dominio;
using Formulador.Data.DataAccess;

namespace Formulador.Data.Repositorios
{
    public class DetalleRepository : BaseReporitory
    {
        public async Task<List<Detalle>> GetByFormula(int formula_id)
        {
            string query = $@"
                    Select * 
                    from FORMULACION.formuladetalle 
                    where IdFormula = {formula_id}
            ";

            List<Detalle> MisDetalles = await DAO.QueryToList<Detalle>(query);

            return MisDetalles;
        }
    }
}
