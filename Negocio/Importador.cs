using System;
using System.Collections.Generic;
using System.Text;
using Formulador.Dominio;
using System.Linq;
using System.Threading.Tasks;
using Formulador.Data.Repositorios;

namespace Formulador.Negocio
{
    public static class Importador
    {
        public async static Task<dynamic> Import(string user)
        {
            ImportRepository _importRepository = new ImportRepository();
            try
            {
                List<Formula> formulas = await _importRepository.FormulasAll(user);

                var f = formulas.Select(x => x.IdFormula.ToString()).ToList();

                List<string> idClientes = formulas.Select(x => x.IdCliente).ToList()
                                            .Where(elem => elem != "" && elem != null).ToList()
                                            .Select(x => $@"'{x}'").ToList();


                var c_joined = String.Join(",", idClientes);

                List<Detalle> detalles = await _importRepository.DetallesPorFormulas(String.Join(",", f));

                List<Cliente> clientes = await _importRepository.ClientesPorFormula(c_joined);

                return new
                {
                    formulas = formulas,
                    detalles = detalles,
                    clientes = clientes
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
