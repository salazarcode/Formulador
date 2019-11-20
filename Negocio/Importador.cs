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
                List<Formula> formulas = await _importRepository.Formulas(user);

                List<Detalle> detalles = await _importRepository.Detalles(user);

                List<Cliente> clientes = await _importRepository.Clientes(user);

                await _importRepository.Guardar(clientes);

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
