using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Attributes;
using SF3.Win.Controls;
using static SF3.Win.Utils.ControlUtils;

namespace SF3.Win.Utils {
    public static class ObjectListViewUtils {
        /// <summary>
        /// Function to use for each EditorCreatorDelegate we're hijacking.
        /// Creates a combo box instead of the standard control if a named value is present.
        /// </summary>
        /// <param name="obj">The object bound to the ObjectListView row.</param>
        /// <param name="model">The column of the OLV.</param>
        /// <param name="value">The value fetched from the column.</param>
        /// <param name="oldDelegate">The EditorCreatorDelegate we're replacing to use as a fallback.</param>
        /// <returns>The control to use when editing - a ComboBox for named values, otherwise the return value of 'oldDelegate'.</returns>
        private static Control NamedValueEditorCreator(object obj, OLVColumn model, object value, EditorCreatorDelegate oldDelegate) {
            var appState = AppState.RetrieveAppState();
            if (appState.UseDropdownsForNamedValues) {
                var nameContext = ((EnhancedObjectListView) model.ListView).NameGetterContext;
                if (nameContext != null) {
                    var property = obj.GetType().GetProperty(model.AspectName);
                    if (property != null) {
                        var attr = property.GetCustomAttribute<NameGetterAttribute>();
                        if (attr != null && nameContext.CanGetInfo(obj, property, attr.Parameters)) {
                            var intValue = Convert.ToInt32(property.GetValue(obj));
                            if (nameContext.CanGetName(obj, property, intValue, attr.Parameters))
                                return MakeNamedValueComboBox(nameContext.GetInfo(obj, property, attr.Parameters), intValue);
                        }
                    }
                }
            }

            return oldDelegate(obj, model, value);
        }

        private static bool _namedValuesRegistered = false;

        /// <summary>
        /// Performs ObjectListView.EditorRegistry.Register() for all SF3 NamedValues.
        /// </summary>
        public static void RegisterNamedValues() {
            if (_namedValuesRegistered == true)
                return;
            _namedValuesRegistered = true;

            /// BIG HACK to get existing editor delegates.
            var creatorMapField = ObjectListView.EditorRegistry.GetType().GetField(
                "creatorMap", BindingFlags.NonPublic | BindingFlags.Instance);
            var creatorMap = (Dictionary<Type, EditorCreatorDelegate>) creatorMapField.GetValue(ObjectListView.EditorRegistry);

            ObjectListView.EditorRegistry.Register(typeof(sbyte), typeof(SByteUpDownControl));
            ObjectListView.EditorRegistry.Register(typeof(byte), typeof(ByteUpDownControl));
            ObjectListView.EditorRegistry.Register(typeof(short), typeof(Int16UpDownControl));
            ObjectListView.EditorRegistry.Register(typeof(ushort), typeof(UInt16UpDownControl));
            ObjectListView.EditorRegistry.Register(typeof(int), typeof(Int32UpDownControl));
            ObjectListView.EditorRegistry.Register(typeof(uint), typeof(UInt32UpDownControl));
            ObjectListView.EditorRegistry.Register(typeof(float), typeof(FloatUpDownControl));

            var typesToHijack = new Type[] {
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
                typeof(float)
            };

            foreach (var type in typesToHijack) {
                var creator = creatorMap[type];
                ObjectListView.EditorRegistry.Register(type, (obj, model, value)
                    => NamedValueEditorCreator(obj, model, value, creator));
            }
        }
    }
}
