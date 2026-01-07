using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Common.Collection
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmptyCollection<T>(this IEnumerable<T> collection)
        {
            return collection is null || collection.Count() == 0;
        }

        public static bool IsNullOrEmptyCollection<T>(this IList<T> collection)
        {
            return collection is null || collection.Count() == 0;
        }

        public static bool IsNullOrEmptyCollection<T>(this List<T> collection)
        {
            return collection is null || collection.Count() == 0;
        }

        public static DataTable ListWithPropertiesToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var prop in props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);

                dataTable.Columns.Add(prop.Name, type);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static DataTable ListWithoutPropertiesToDataTable<T>(this List<T> items, string columnName)
        {
            var type = typeof(T);

            DataTable dataTable = new();

            dataTable.Columns.Add(columnName, type);
            foreach (var item in items)
                dataTable.Rows.Add(item);

            return dataTable;
        }

        public static List<T> DataTableToList<T>(this DataTable dataTable)
        {
            var dt = dataTable;

            List<T> data = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }

            return data;
        }

        public static List<dynamic> DataTableToDynamic(this DataTable dataTable)
        {
            var dt = dataTable;

            var result = new List<dynamic>();

            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                result.Add(dyn);

                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    if (dt.Columns.Contains(column.ColumnName))
                        dic[column.ColumnName] = row[dt.Columns[column.ColumnName]];
                    else
                        dic[column.ColumnName] = 0;

                }
            }

            return result;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        object value = dr[column.ColumnName];
                        if (value == DBNull.Value)
                            value = null;

                        pro.SetValue(obj, value, null);
                    }
                    else
                        continue;
                }
            }

            return obj;
        }
    }
}
