using ProgressiveRate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ProgressiveRate.Services
{
    public class CargoManager
    {
        public Cargo[] DataTableToCargos(DataTable table)
        {
            var cargos = new List<Cargo>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                var cargo = (Cargo)Activator.CreateInstance(typeof(Cargo));
                var properties = typeof(Cargo).GetProperties().Where(p => p.GetCustomAttribute<DisplayNameAttribute>() != null);

                foreach (var property in properties)
                {
                    Type pType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    var cellValue = table.Rows[i][property.GetCustomAttribute<DisplayNameAttribute>().DisplayName];

                    var value = cellValue != null && cellValue != DBNull.Value ?
                                        Convert.ChangeType(cellValue, pType) :
                                        null;

                    property.SetValue(cargo, value);
                }

                cargos.Add(cargo);
            }

            return cargos.ToArray();
        }
    }
}
