using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Dominio;
using Formulador.Data.DataAccess;
using Formulador.Transversal;
using System.Reflection;
using System.Linq;

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
        public async Task<List<Articulo>> Articulos()
        {
            try
            {
                string query = $@"
                        select * 
                        from 
                            exactusdb.syn_Articulo 
                        where 
                            articulo like '%01.13'
                            or articulo like '%01.14'
                            or articulo like '%01.26'
                            or articulo like '%01.34'
                            or articulo like '%02.13'
                            or articulo like '%02.14'
                            or articulo like '%01.62'
                ";

                List<Articulo> res = await RemoteDAO.QueryToList<Articulo>(query);

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
                        Select 
                            cliente, nombre 
                        from 
                            EXACTUSDB.SYN_CLIENTE 
                        where cliente in(
                            SELECT 
                                IdCliente 
                            FROM 
                                FORMULACION.formula 
                            where 
                                UsuarioRegistro like '%{user}%'
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
                await GuardarLocalmente<Cliente>(clientes, "Cliente");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Guardar(List<Formula> formulas)
        {
            try
            {
                await GuardarLocalmente<Formula>(formulas, "Formula");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Guardar(List<Articulo> articulos)
        {
            try
            {
                await GuardarLocalmente<Articulo>(articulos, "Articulo");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Guardar(List<Detalle> detalles)
        {
            try
            {
                await GuardarLocalmente<Detalle>(detalles, "FormulaDetalle");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task GuardarLocalmente<T>(List<T> insumo, string TableName)
        {
            try
            {
                var n = await LocalDAO.Query($@"delete from {TableName}");

                List<string> props = typeof(T).GetProperties().ToList().Select(x => x.Name).ToList();

                Parallel.ForEach(insumo, async x => {
                    string query = $@"
                        insert into 
                        {TableName}({String.Join(",", props)})
                        values({String.Join(",", props.Select(x => $"@{x}").ToList())}); 
                    ";

                    var parametros = new Dictionary<string, dynamic>();

                    props.ForEach(prop => parametros.Add($"@{prop}", x.GetType().GetProperty(prop).GetValue(x, null)));

                    var res = await LocalDAO.QueryWithParam(query, parametros);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task PullChanges(string TableName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
