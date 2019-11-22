using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Dominio;
using Formulador.Data.DataAccess;
using Formulador.Transversal;
using Formulador.Transversal.DTO;
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

        public async Task<List<Articulo>> PullChanges_Articulos()
        {
            try
            {
                List<Articulo> articulos = await LocalDAO.QueryToList<Articulo>("select Articulo, FCH_HORA_ULT_MODIF from Articulo");
                List<Insumo> insumo = articulos.Select(x => new Insumo
                {
                    id = x.ARTICULO,
                    UltimaModificacion = x.FCH_HORA_ULT_MODIF.ToString("o").Substring(0,23)
                }).ToList();

                List<Articulo> actualizados = new List<Articulo>();

                var insumosPaginados = insumo.PaginateList(500);

                insumosPaginados.ForEach(async lista => {
                    string query = "FORMULACION.usp_Articulos_Diferencias";

                    //PREGUNTO A LA BBDD SI ESTOS REGISTROS CAMBIARON Y OBTENGO SOLO LOS QUE CAMBIARON

                    var concatenados = ConcatInsumo(lista);
                    var parametros = new Dictionary<string, dynamic>();
                    parametros.Add($"@insumo", concatenados + ";");

                    List<Articulo> res = RemoteDAO.QueryToList<Articulo>(query, parametros, true);

                    //SI HUBO RESULTADOS ELIMINO LOS LOCALES CON ESOS IDS
                    if (res.Count != 0)
                    {
                        res.ForEach(x => actualizados.Add(x));
                        //ELIMINO EN LOCAL
                        var parametrosDelete = new Dictionary<string, dynamic>();
                        parametrosDelete.Add("@articulos", String.Join(",", res.Select(z => z.ARTICULO).ToList()));
                        await LocalDAO.QueryWithParam("delete from Articulo where articulo in(@articulos)", parametrosDelete);

                        //GUARDO LA VERSIÓN MÁS RECIENTE QUE ESTABA EN LA BASE DE DATOS.
                        await Guardar(res);
                    }
                });

                return actualizados;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static string ConcatInsumo(List<Insumo> lista)
        {
            var concatenado = lista.Select(x => $"{x.id},{x.UltimaModificacion.ToString()}").ToList();

            return String.Join(";", concatenado);
        }


    }
}
