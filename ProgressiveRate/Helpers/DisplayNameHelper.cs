using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ProgressiveRate.Helpers
{
    public static class DisplayNameHelper
    {
        public static void FillNames(Type type, Dictionary<string, string> names)
        {
            names.Clear();

            var properties = type.GetProperties().Where(x => x.GetCustomAttribute<DisplayNameAttribute>() != null);

            foreach (var property in properties)
                names.Add(property.Name, property.GetCustomAttribute<DisplayNameAttribute>().DisplayName);
        }
    }
}
