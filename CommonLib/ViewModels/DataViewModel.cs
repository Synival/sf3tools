using System.Collections.Generic;
using System.Reflection;
using CommonLib.Attributes;

namespace CommonLib.ViewModels {
    public class DataViewModel {
        public DataViewModel(Dictionary<PropertyInfo, DataViewModelColumnAttribute> properties) {
            Properties = properties;
        }

        public Dictionary<PropertyInfo, DataViewModelColumnAttribute> Properties { get; }
    }
}
