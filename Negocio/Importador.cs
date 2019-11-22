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
                /*
                List<Formula> formulas = await _importRepository.Formulas(user);
                List<Detalle> detalles = await _importRepository.Detalles(user);
                List<Cliente> clientes = await _importRepository.Clientes(user);
                List<Articulo> articulos = await _importRepository.Articulos();

                await _importRepository.Guardar(clientes);
                await _importRepository.Guardar(formulas);
                await _importRepository.Guardar(detalles);

                return new
                {
                    formulas = formulas,
                    detalles = detalles,
                    clientes = clientes
                };
                */
                List<Articulo> articulos = await _importRepository.Articulos();
                await _importRepository.Guardar(articulos);
                return articulos;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
