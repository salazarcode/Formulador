using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Formulador.Data.Repositorios;

namespace Formulador.Dominio
{
    public class TiposProducto
    {
        public TiposProductoRepository _TiposProductoRepository;

        public TiposProducto()
        {
            _TiposProductoRepository = new TiposProductoRepository();
        }
        public int IdTipoProducto { get; set; }
        public string Nombre { get; set; }
        public bool EsActivo { get; set; }
        public static async Task<List<TiposProducto>> All()
        {
            var _TiposProductoRepository = new TiposProductoRepository();
            return await _TiposProductoRepository.All();
        }
    }
}
