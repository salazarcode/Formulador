﻿using System;
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

        public List<T> QueryToList<T>(string query, IDictionary<string, dynamic> parameters, bool isProcedure = false) where T : new()
        {
            List<T> res = null;
            try
            {
                var conn = GetConnection();
                var command = new SqlCommand(query, conn);
                if(isProcedure)
                    command.CommandType = CommandType.StoredProcedure;

                parameters.ToList().ForEach(x => {
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = x.Key,
                        Value = x.Value
                    });
                });

                //await conn.OpenAsync();
                conn.Open();
                var reader = command.ExecuteReader();
                res = reader.ToCustomList<T>();
            }
            catch (Exception ex)
            {
                var error = new
                {
                    Message = ex.Message,
                    Parameter = parameters.First().Value
                };
            }

            return res;
        }


        public async Task<int> QueryWithParam(string query, IDictionary<string, dynamic> parameters, bool isProcedure = false)
        {
            try
            {
                using var conn = GetConnection();
                using var command = new SqlCommand(query, conn);
                if (isProcedure)
                    command.CommandType = CommandType.StoredProcedure;

                parameters.ToList().ForEach(x => {
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = x.Key,
                        Value = x.Value
                    });
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
