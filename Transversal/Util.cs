using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Formulador.Transversal
{
    public static class Util
    {
        public static List<T> ToCustomList<T>(this IDataReader dataReader) where T : new()
        {
            DataTable dt = new DataTable();
            List<T> lista = new List<T>();

            try
            {                
                dt.Load(dataReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            foreach (DataRow dr in dt.Rows)
            {
                T obj = Util.ToCustomObject<T>(dr);
                lista.Add(obj);
            }
            return lista;
        }
        public static T ToCustomObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                try
                {
                    PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                    if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                    {
                        try
                        {
                            property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return item;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                 .FirstOrDefault();
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }
    }
}
