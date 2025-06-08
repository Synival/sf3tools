using System.Reflection;
using CommonLib.ViewModels;

namespace SF3.Win.Views {
    /// <summary>
    /// Item row representing an individual property with its own view model.
    /// </summary>
    public class ModelProperty {
        public ModelProperty(object model, PropertyInfo propertyInfo, TableViewModelColumn vmColumn, int index) {
            Model                = model;
            PropertyInfo         = propertyInfo;
            VMColumn             = vmColumn;
            Index                = index;
            IsReadOnly           = !vmColumn.GetColumnIsEditable(propertyInfo);
            Name                 = vmColumn.GetColumnText(propertyInfo);
            AspectToStringFormat = vmColumn.GetColumnAspectToStringFormat();
            Width                = vmColumn.GetColumnWidth();
        }

        public object Model { get; }
        public PropertyInfo PropertyInfo { get; }
        public TableViewModelColumn VMColumn { get; }
        public int Index { get; }

        public bool IsReadOnly { get; }
        public string Name { get; }
        public string AspectToStringFormat { get; }
        public int Width { get; }

        public object Value {
            get => PropertyInfo.GetValue(Model, null);
            set {
                if (!IsReadOnly)
                    PropertyInfo.SetValue(Model, value);
            }
        }
    };
}
