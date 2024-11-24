using System.Collections.Generic;
using System.Reflection;
using SF3.Attributes;

namespace SF3.ViewModels {
    public class DataViewModel {
        public DataViewModel(Dictionary<PropertyInfo, ViewModelDataAttribute> properties) {
            Properties = properties;
        }

        public Dictionary<PropertyInfo, ViewModelDataAttribute> Properties { get; }
    }
}
