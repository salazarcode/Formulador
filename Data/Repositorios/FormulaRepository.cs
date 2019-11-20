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
    public class FormulaRepository : ServiceContainer
    {
        public async Task<List<Formula>> All(string user)
        {
            string query = $@"SELECT 
                                    F.IDFORMULA,
                                    F.ARTICULO,
                                    F.CODTEMPORAL,
                                    F.NOMBRE,
                                    F.IDCLIENTE,
                                    F.IDTIPOPRODUCTO,
                                    F.CODDIV,
                                    F.CODFAM,
                                    F.CODSUBFAM,
                                    F.ESOFICIAL,
                                    F.OBSERVACIONES,
                                    TIPOPRODUCTOSTR = TP.NOMBRE,
                                    CLIENTESTR = C.NOMBRE,
                                    F.FECHAREGISTRO,
                                    F.USUARIOREGISTRO,	
                                    ESGRANEL = MONTANAINTERNOS.FORMULACION.UF_VERIFYISGRANEL(F.ARTICULO), 
                                    F.ESEMPAQUE 
                                FROM 
                                    MONTANAINTERNOS.FORMULACION.FORMULA F WITH(NOLOCK) 
                                    INNER JOIN MONTANAINTERNOS.FORMULACION.TIPOPRODUCTO TP WITH(NOLOCK) 
                                        ON TP.IDTIPOPRODUCTO = F.IDTIPOPRODUCTO
                                    LEFT JOIN MONTANAINTERNOS.EXACTUSDB.SYN_CLIENTE C WITH(NOLOCK) 
                                        ON C.CLIENTE = F.IDCLIENTE
                                where F.UsuarioRegistro like '%{user}%'
            ";

            List<Formula> Misformulas = await LocalDAO.QueryToList<Formula>(query);
            return Misformulas;
        }
    }
}
