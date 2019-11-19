using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Dominio;
using Formulador.Data.DataAccess;
using AutoMapper;
using System.Data;
using System.Linq;
using AutoMapper.Data.Mappers;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Formulador.Transversal;

namespace Formulador.Data.Repositorios
{
    public class TiposProductoRepository : BaseReporitory
    {
        public async Task<List<TiposProducto>> All()
        {
            string query = $@"SELECT * FROM MONTANAINTERNOS.FORMULACION.TIPOPRODUCTO WHERE ESACTIVO = 1";

            List<TiposProducto> tiposProducto = await DAO.QueryToList<TiposProducto>(query);
            return tiposProducto;
        }
    }
}
