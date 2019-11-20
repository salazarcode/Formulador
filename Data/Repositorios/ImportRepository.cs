using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Dominio;
using Formulador.Data.DataAccess;
using Formulador.Transversal;

namespace Formulador.Data.Repositorios
{
    public class ImportRepository : ServiceContainer
    {
        public async Task<List<Formula>> Formulas(string user)
        {
            try
            {
                string query = $@"SELECT * FROM FORMULACION.formula where UsuarioRegistro like '%{user}%'";

                List<Formula> res = await RemoteDAO.QueryToList<Formula>(query);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<Detalle>> Detalles(string user)
        {
            try
            {
                string query = $@"
                        Select * 
                        from FORMULACION.formuladetalle 
                        where IdFormula in(
                            SELECT idFormula 
                            FROM FORMULACION.formula 
                            where UsuarioRegistro like '%{user}%'
                        )
                ";

                List<Detalle> res = await RemoteDAO.QueryToList<Detalle>(query);

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<Cliente>> Clientes(string user)
        {
            try
            {
                string query = $@"
                        Select cliente, nombre 
                        from EXACTUSDB.SYN_CLIENTE 
                        where cliente in(
                            SELECT IdCliente 
                            FROM FORMULACION.formula 
                            where UsuarioRegistro like '%{user}%'
                        )
                ";

                List<Cliente> res = await RemoteDAO.QueryToList<Cliente>(query);

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task Guardar(List<Cliente> clientes)
        {
            try
            {
                var n = await LocalDAO.Query($@"delete from CLIENTE");

                //List<string> queries = new List<string>();

                foreach (var x in clientes)
                {
                    try
                    {
                        await LocalDAO.InsertCliente($"insert into cliente(CLIENTE, NOMBRE) values('@cliente', '@nombre'); ", new { 
                            cliente= x.cliente,
                            nombre = x.nombre
                        });
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /*
         * public async Task Checking()
        {
            try
            {
                var db = LocalDAO.GetConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        */
    }
}
