using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ProgressiveRate.Services
{
    public static class DataTableMapper
    {
        public static T[] MapTo<T>(DataTable table)
        {
            var items = new List<T>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                var cargo = (T)Activator.CreateInstance(typeof(T));
                var properties = typeof(T).GetProperties().Where(p => p.GetCustomAttribute<DisplayNameAttribute>() != null);

                foreach (var property in properties)
                {
                    Type pType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    var cellValue = table.Rows[i][property.GetCustomAttribute<DisplayNameAttribute>().DisplayName];

                    var value = cellValue != null && cellValue != DBNull.Value ?
                                        Convert.ChangeType(cellValue, pType) :
                                        null;

                    property.SetValue(cargo, value);
                }

                items.Add(cargo);
            }

            return items.ToArray();
        }
    }
}
