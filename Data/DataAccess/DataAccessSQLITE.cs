using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Configuration;
using Formulador.Transversal;
using System.Linq;

namespace Formulador.Data.DataAccess
{
    public class DataAccessSQLITE
    {
        private string connStr;
        public delegate List<T> Mapping<T>(SQLiteDataReader reader);

        public DataAccessSQLITE()
        {
            connStr = ConfigurationManager.ConnectionStrings["SqliteStr"].ConnectionString;
        }

        public SQLiteConnection GetConnection() 
        {
            SQLiteConnection conn = new SQLiteConnection(connStr);
            return conn;
        }

        public async Task<List<T>> QueryWithReaderHandler<T>(string query, Mapping<T> mapper)
        {
            try
            {
                using var conn = GetConnection();
                using var command = new SQLiteCommand(query, conn);
                await conn.OpenAsync();
                using var reader = command.ExecuteReader();
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
                using var conn = GetConnection();
                using var command = new SQLiteCommand(query, conn);
                await conn.OpenAsync();
                using var reader = command.ExecuteReader();
                res = reader.ToList<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        public async Task<int> Query(string query)
        {
            try
            {
                using var conn = GetConnection();
                using var command = new SQLiteCommand(query, conn);
                await conn.OpenAsync();

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> InsertCliente(string query, dynamic parameters)
        {
            try
            {
                /*
                
                using var command = new SQLiteCommand(query, conn);
                await conn.OpenAsync();
                */
                using var conn = GetConnection();
                using var command = new SQLiteCommand(query, conn);

                command.Parameters.Add(new SQLiteParameter
                {
                    ParameterName = "@cliente",
                    Value = parameters.cliente
                });

                command.Parameters.Add(new SQLiteParameter
                {
                    ParameterName = "@nombre",
                    Value = parameters.nombre
                });

                conn.Open();

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
