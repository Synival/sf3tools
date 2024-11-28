using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.ViewModels;

namespace CommonLib.Extensions {
    public static class TypeExtensions {
        private static Dictionary<string, TableViewModel> _cachedTableViewModels = new Dictionary<string, TableViewModel>();

        /// <summary>
        /// Retrieves the auto-generated TableViewModel for the specified type by reflecting on TableViewModelColumnAttribute's.
        /// </summary>
        /// <param name="type">The type whose TableViewModel to retrieve.</param>
        /// <returns>A reference to a unique TableViewModel instance associated with 'type'.</returns>
        public static TableViewModel GetTableViewModel(this Type type) {
            if (_cachedTableViewModels.ContainsKey(type.AssemblyQualifiedName))
                return _cachedTableViewModels[type.AssemblyQualifiedName];

            var columnProperties = type
                .GetProperties()
                // TODO: this should actually order by inheriency chain, not "is this me?"
                .OrderBy(x => x.DeclaringType == type ? 1 : 0)
                .ToList();

            var props = columnProperties
                .Select(x => new { Property = x, Attributes = x.GetCustomAttributes(typeof(TableViewModelColumnAttribute), true) })
                .Where(x => x.Attributes.Length == 1)
                .OrderBy(x => ((TableViewModelColumnAttribute) x.Attributes[0]).Column.DisplayOrder)
                .ToDictionary(x => x.Property, x => ((TableViewModelColumnAttribute) x.Attributes[0]).Column);

            var vm = new TableViewModel(props);
            _cachedTableViewModels.Add(type.AssemblyQualifiedName, vm);
            return vm;
        }
    }
}
