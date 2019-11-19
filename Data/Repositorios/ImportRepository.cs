using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Dominio;
using Formulador.Data.DataAccess;

namespace Formulador.Data.Repositorios
{
    public class ImportRepository : BaseReporitory
    {
        public async Task<List<Formula>> FormulasAll(string user)
        {
            string query = $@"SELECT * FROM FORMULACION.formula where UsuarioRegistro like '%{user}%'";

            List<Formula> res = await DAO.QueryToList<Formula>(query);
            return res;
        }
        public async Task<List<Detalle>> DetallesPorFormulas(string formula_ids)
        {
            string query = $@"
                    Select * 
                    from FORMULACION.formuladetalle 
                    where IdFormula in({formula_ids})
            ";

            List<Detalle> res = await DAO.QueryToList<Detalle>(query);

            return res;
        }
        public async Task<List<Cliente>> ClientesPorFormula(string cliente_ids)
        {
            string query = $@"
                    Select cliente, nombre 
                    from EXACTUSDB.SYN_CLIENTE 
                    where cliente in({cliente_ids})
            ";

            List<Cliente> res = await DAO.QueryToList<Cliente>(query);

            return res;
        }
    }
}
