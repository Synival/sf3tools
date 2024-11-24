﻿using System;
using System.Collections.Generic;
using System.Linq;
using SF3.Attributes;
using SF3.ViewModels;

namespace SF3.Extensions {
    public static class TypeExtensions {
        private static Dictionary<string, DataViewModel> _cachedDataViewModels = new Dictionary<string, DataViewModel>();

        /// <summary>
        /// Retrieves the auto-generated DataViewModel for the specified type by reflecting on ViewModelDataAttribute's.
        /// </summary>
        /// <param name="type">The type whose DataViewModel to retrieve.</param>
        /// <returns>A reference to a unique DataViewModel instance associated with 'type'.</returns>
        public static DataViewModel GetDataViewModel(this Type type) {
            if (_cachedDataViewModels.ContainsKey(type.AssemblyQualifiedName))
                return _cachedDataViewModels[type.AssemblyQualifiedName];

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

            var vm = new DataViewModel(props);
            _cachedDataViewModels.Add(type.AssemblyQualifiedName, vm);
            return vm;
        }
    }
}
