using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF3.Attributes;
using SF3.ViewModels;

namespace SF3.Extensions {
    public static class TypeExtensions {
        public static DataViewModel CreateDataViewModel(this Type type) {
            var columnProperties = type
                .GetProperties()
                // TODO: this should actually order by inheriency chain, not "is this me?"
                .OrderBy(x => x.DeclaringType == type ? 1 : 0)
                .ToList();

            var props = columnProperties
                .Select(x => new { Property = x, Attributes = x.GetCustomAttributes(typeof(ViewModelDataAttribute), true) })
                .Where(x => x.Attributes.Length == 1)
                .OrderBy(x => ((ViewModelDataAttribute) x.Attributes[0]).DisplayOrder)
                .ToDictionary(x => x.Property, x => (ViewModelDataAttribute) x.Attributes[0]);

            return new DataViewModel(props);
        }
    }
}
