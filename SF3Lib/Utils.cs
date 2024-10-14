using SF3.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SF3
{
    /// <summary>
    /// Miscellaneous utility functions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Returns a string with a stringified hex value followed by a value name if available in a dictionary.
        /// </summary>
        /// <param name="value">Value for which a string should be generated</param>
        /// <param name="nameDict">Dictionary with possible names for 'value'</param>
        /// <param name="hexDigits">Number of digits for the hexideimcal portion of the return string</param>
        /// <returns>
        /// If no name available:
        ///     {0:X[hexDigits]}
        /// If value name available:
        ///     {0:X[hexDigits]: [name]
        /// </returns>
        public static string HexValueWithName(int value, Dictionary<int, string> nameDict, int hexDigits = 2)
        {
            var valueStr = value.ToString("X" + hexDigits.ToString());
            string className;
            return nameDict.TryGetValue(value, out className)
                ? valueStr + ": " + className
                : valueStr;
        }

        /// <summary>
        /// Creates a dictionary of all possible values to supply to MakeNamedValueComboBox().
        /// </summary>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <param name="factoryFunc">Factory function to create a NamedValue. Used for looking up 'NamedValue.Name' for all possible values.</param>
        /// <returns>A key-value dictionary of all possible values (key) and their names (value).</returns>
        public static Dictionary<NamedValue, string> MakeNamedValueComboBoxValues(int minValue, int maxValue, Func<int, NamedValue> factoryFunc)
        {
            var dict = new Dictionary<NamedValue, string>();
            for (int i = minValue; i < maxValue; i++)
            {
                var value = factoryFunc(i);
                dict.Add(value, value.Name);
            }
            return dict;
        }

        /// <summary>
        /// Result for individual properties affected by BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyPropertyResult
        {
            public BulkCopyPropertyResult(PropertyInfo property, object oldValue, object newValue)
            {
                Property = property;
                OldValue = oldValue;
                NewValue = newValue;
                Changed = !NewValue.Equals(OldValue);
            }

            /// <summary>
            /// The property affected.
            /// </summary>
            public PropertyInfo Property { get; }

            /// <summary>
            /// The old value of the property before the copy.
            /// </summary>
            public object OldValue { get; }

            /// <summary>
            /// The new value of the property after the copy.
            /// </summary>
            public object NewValue { get; }

            /// <summary>
            /// True if the property was modified.
            /// </summary>
            public bool Changed { get; }
        }

        /// <summary>
        /// Collection of BulkCopyPropertyResult's reported by BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyPropertiesResult
        {
            public BulkCopyPropertiesResult(IEnumerable<BulkCopyPropertyResult> properties)
            {
                Properties = properties;
            }

            /// <summary>
            /// Collection of properties affected.
            /// </summary>
            public IEnumerable<BulkCopyPropertyResult> Properties { get; }
        }

        /// <summary>
        /// Result for an individual row in BulkCopyCollectionResult.
        /// </summary>
        public class BulkCopyCollectionRowResult<T> where T : class
        {
            /// <summary>
            /// Bulk copy result for a row that was or was not copied, depending on whether 'copyResult' is 'null'.
            /// </summary>
            /// <param name="index">Index of the row in 'listFrom'.</param>
            /// <param name="copyResult">Result of the row copied. Can be 'null' to indicate the row wasn't copied.</param>
            public BulkCopyCollectionRowResult(int index, T rowFrom, T rowTo, BulkCopyPropertiesResult copyResult)
            {
                Index = index;
                RowFrom = rowFrom;
                RowTo = rowTo;
                Copied = (rowTo != null && copyResult != null);
                CopyResult = copyResult;
            }

            /// <summary>
            /// The index of the row in the 'listFrom' collection.
            /// </summary>
            public int Index { get; }

            /// <summary>
            /// The input row.
            /// </summary>
            public T RowFrom { get; }

            /// <summary>
            /// The output row.
            /// </summary>
            public T RowTo { get; }

            /// <summary>
            /// 'True' if this row was copied.
            /// </summary>
            public bool Copied { get; }

            /// <summary>
            /// Report for all properties copied. Can be 'null' if this row was not copied (i.e, 'Copied' is 'false').
            /// </summary>
            public BulkCopyPropertiesResult CopyResult { get; }
        }

        /// <summary>
        /// Result for BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyCollectionResult<T> where T : class
        {
            public BulkCopyCollectionResult(IEnumerable<BulkCopyCollectionRowResult<T>> inputRows, int listOutRowsIgnored)
            {
                InputRows = inputRows;
                InRowsSkipped = inputRows.Count(x => !x.Copied);
                OutRowsSkipped = listOutRowsIgnored;
                RowsCopied = inputRows.Count(x => x.Copied);
            }

            /// <summary>
            /// Individual reports for each row in 'listForm'.
            /// </summary>
            public IEnumerable<BulkCopyCollectionRowResult<T>> InputRows { get; }

            /// <summary>
            /// The number of rows in 'objFrom' that were not copied to 'objTo'.
            /// </summary>
            public int InRowsSkipped { get; }

            /// <summary>
            /// The number of rows in 'objTo' that were unaffected.
            /// </summary>
            public int OutRowsSkipped { get; }

            /// <summary>
            /// The number of rows copied from 'objFrom' to 'objTo'.
            /// </summary>
            public int RowsCopied { get; }

            /// <summary>
            /// Produces a large report with overall/summary and individual changes.
            /// </summary>
            /// <returns>A multiline string with a trailing "\n".</returns>
            public string MakeFullReport()
            {
                string report =
                    "Overall report:\n" +
                    "====================================================\n" +
                    MakeSummaryReport();

                report += "\n\n" +
                    "Individual changes report:\n" +
                    "====================================================\n" +
                    MakeIndividualChangesReport();

                return report;
            }

            /// <summary>
            /// Produces a summary report for changes made to the list.
            /// </summary>
            /// <returns>A multiline string with a trailing "\n".</returns>
            public string MakeSummaryReport()
            {
                string report =
                    "Rows copied: " + RowsCopied + "\n" +
                    "Input rows skipped: " + InRowsSkipped + "\n" +
                    "Target rows unaffected: " + OutRowsSkipped + "\n";

                var rowsWithChanges = InputRows.Where(x => x.Copied && x.CopyResult.Properties.Any(y => y.Changed)).ToList();
                report += "Rows changed: " + rowsWithChanges.Count + "\n";

                var cellsChanged = rowsWithChanges.Sum(x => x.CopyResult.Properties.Count(y => y.Changed));
                report += "Cells changed: " + cellsChanged + "\n";

                return report;
            }

            /// <summary>
            /// Produces a detailed report of all changes in the list.
            /// </summary>
            /// <returns>A multiline string with a trailing "\n".</returns>
            public string MakeIndividualChangesReport()
            {
                string report = "";
                var rowsWithChanges = InputRows.Where(x => x.Copied && x.CopyResult.Properties.Any(y => y.Changed)).ToList();

                foreach (var row in rowsWithChanges)
                {
                    report += "Row [" + row.Index + "]:\n";
                    var changes = row.CopyResult.Properties.Where(x => x.Changed).ToList();
                    foreach (var change in changes)
                    {
                        report += "    " + change.Property.Name + ": " + change.OldValue.ToString() + " => " + change.NewValue.ToString() + "\n";
                    }
                }

                return report;
            }
        }

        /// <summary>
        /// Copies all properties of type T tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <typeparam name="T">The type whose properties should be </typeparam>
        /// <param name="objFrom">The object whose properties should be copied from.</param>
        /// <param name="objTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A list of all properties considered and the result.</returns>
        public static BulkCopyPropertiesResult BulkCopyProperties<T>(T objFrom, T objTo, bool inherit = true) where T : class
        {
            // Get all public properties with the [BulkCopy] attribute.
            var properties = typeof(T).GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    (inherit ? 0 : BindingFlags.DeclaredOnly)
                )
                .Where(x => x.IsDefined(typeof(BulkCopyAttribute)))
                .ToList();

            var propertyList = new List<BulkCopyPropertyResult>();
            foreach (var property in properties)
            {
                var oldValue = property.GetValue(objTo);
                property.SetValue(objTo, property.GetValue(objFrom));
                var newValue = property.GetValue(objTo);

                propertyList.Add(new BulkCopyPropertyResult(property, oldValue, newValue));
            }

            return new BulkCopyPropertiesResult(propertyList);
        }

        /// <summary>
        /// Copies all properties of type T in an IEnumerable<T> tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <typeparam name="T">The type whose properties should be </typeparam>
        /// <param name="listFrom">The object whose properties should be copied from.</param>
        /// <param name="listTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A report with the number of rows affected, unaffected, and each row's individual properties changed.</returns>
        public static BulkCopyCollectionResult<T> BulkCopyCollectionProperties<T>(IEnumerable<T> listFrom, IEnumerable<T> listTo, bool inherit = true) where T : class
        {
            // Get all public properties with the [BulkCopy] attribute.
            var properties = typeof(T).GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    (inherit ? 0 : BindingFlags.DeclaredOnly))
                .Where(x => x.IsDefined(typeof(BulkCopyAttribute)))
                .ToList();

            var arrayFrom = listFrom.ToArray();
            var arrayTo = listTo.ToArray();

            var inputRowReports = new List<BulkCopyCollectionRowResult<T>>();
            for (int i = 0; i < arrayFrom.Length; i++)
            {
                var rowTo = (i < arrayTo.Length) ? arrayTo[i] : null;
                inputRowReports.Add(new BulkCopyCollectionRowResult<T>(i, arrayFrom[i], rowTo,
                    (rowTo != null) ? BulkCopyProperties(arrayFrom[i], rowTo, inherit) : null));
            }

            return new BulkCopyCollectionResult<T>(inputRowReports, Math.Max(arrayTo.Length - arrayFrom.Length, 0));
        }

        /// <summary>
        /// Gets all values of all fields with type T.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="obj">The object whose fields should be searched.</param>
        /// <param name="inherit">If 'true', values for fields with base classes will be returned.</param>
        /// <returns></returns>
        public static List<T> GetAllObjectsOfTypeInFields<T>(object obj, bool inherit = true) where T : class
        {
            var fields = obj.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                (inherit ? 0 : BindingFlags.DeclaredOnly)
            );

            return fields
                .Where(x => x.FieldType == typeof(T))
                .Select(x => x.GetValue(obj) as T)
                .ToList();
        }
    }
}
