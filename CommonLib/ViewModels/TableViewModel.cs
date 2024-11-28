using System.Collections.Generic;
using System.Reflection;

namespace CommonLib.ViewModels {
    public class TableViewModel {
        public TableViewModel(Dictionary<PropertyInfo, TableViewModelColumn> properties) {
            Properties = properties;
        }

        public Dictionary<PropertyInfo, TableViewModelColumn> Properties { get; }
    }
}
