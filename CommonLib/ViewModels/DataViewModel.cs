using System.Collections.Generic;
using System.Reflection;

namespace CommonLib.ViewModels {
    public class DataViewModel {
        public DataViewModel(Dictionary<PropertyInfo, DataViewModelColumn> properties) {
            Properties = properties;
        }

        public Dictionary<PropertyInfo, DataViewModelColumn> Properties { get; }
    }
}
