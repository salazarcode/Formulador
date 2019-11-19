using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Configuration;
using Formulador.Transversal;

namespace Formulador.Data.DataAccess
{
    public class DataAccess
    {
        private string SqlServerStr;
        private string SqliteStr;
        public delegate List<T> Mapping<T>(SqlDataReader reader);

        public DataAccess()
        {
            SqlServerStr = ConfigurationManager.ConnectionStrings["SqlServerStr"].ConnectionString;
            SqliteStr = ConfigurationManager.ConnectionStrings["SqliteStr"].ConnectionString;
        }

        public SqlConnection GetSqlServerConnection() 
        {
            SqlConnection conn = new SqlConnection(SqlServerStr);
            return conn;
        }

        public async Task<List<T>> QueryWithReaderHandler<T>(bool local, string query, Mapping<T> mapper)
        {
            IDbConnection conn = local ? new SQLiteConnection(sqlitest);
            SqlCommand command = null;
            SqlDataReader reader = null;
            try
            {
                if (local)
                {
                    SQLITE = new SQLiteConnection(SqliteStr);
                }
                else
                {
                    SQLSERVER = new SqlConnection(SqlServerStr);
                }
                command = new SqlCommand(query, local ? SQLITE : SQLSERVER);
                await conn.OpenAsync();
                reader = await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mapper(reader);
        }

        public async Task<List<T>> QueryToList<T>(string query) where T : new()
        {
            SqlConnection conn = null;
            SqlCommand command = null;
            SqlDataReader reader = null;
            List<T> res = null;
            try
            {
                conn = new SqlConnection(SqlServerStr);
                command = new SqlCommand(query, conn);
                await conn.OpenAsync();
                reader = await command.ExecuteReaderAsync();
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
