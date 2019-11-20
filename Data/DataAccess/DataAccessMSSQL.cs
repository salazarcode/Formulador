using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Configuration;
using Formulador.Transversal;
using System.Linq;

namespace Formulador.Data.DataAccess
{
    public class DataAccessMSSQL
    {
        private string connStr;
        public delegate List<T> Mapping<T>(SqlDataReader reader);

        public DataAccessMSSQL()
        {
            connStr = ConfigurationManager.ConnectionStrings["SqlServerStr"].ConnectionString;
        }

        public SqlConnection GetConnection() 
        {
            SqlConnection conn = new SqlConnection(connStr);
            return conn;
        }

        public async Task<List<T>> QueryWithReaderHandler<T>(string query, Mapping<T> mapper)
        {
            try
            {
                var conn = GetConnection();
                var command = new SqlCommand(query, conn);
                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                return mapper(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public async Task<List<T>> QueryToList<T>(string query) where T : new()
        {
            List<T> res = null;
            try
            {
                var conn = GetConnection();
                var command = new SqlCommand(query, conn);
                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                res = reader.ToList<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }
    }
}
