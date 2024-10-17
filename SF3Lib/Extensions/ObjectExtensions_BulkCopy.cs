using SF3.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Utils
{
    public static partial class ObjectExtensions
    {
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
        public interface IBulkCopyCollectionResult
        {
            /// <summary>
            /// The number of rows in 'objFrom' that were not copied to 'objTo'.
            /// </summary>
            int InRowsSkipped { get; }

            /// <summary>
            /// The number of rows in 'objTo' that were unaffected.
            /// </summary>
            int OutRowsSkipped { get; }

            /// <summary>
            /// The number of rows copied from 'objFrom' to 'objTo'.
            /// </summary>
            int RowsCopied { get; }

            /// <summary>
            /// Produces a large report with overall/summary and individual changes.
            /// </summary>
            /// <returns>A multiline string with a trailing "\n".</returns>
            string MakeFullReport();

            /// <summary>
            /// Produces a summary report for changes made to the list.
            /// </summary>
            /// <returns>A multiline string with a trailing "\n".</returns>
            string MakeSummaryReport();

            /// <summary>
            /// Produces a detailed report of all changes in the list.
            /// </summary>
            /// <returns>A multiline string with a trailing "\n".</returns>
            string MakeIndividualChangesReport();
        }

        /// <summary>
        /// Result for BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyCollectionResult<T> : IBulkCopyCollectionResult where T : class
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

            public int InRowsSkipped { get; }
            public int OutRowsSkipped { get; }
            public int RowsCopied { get; }

            public string MakeFullReport()
            {
                string report =
                    "Overall report:\n" +
                    "----------------------------------------------------\n" +
                    MakeSummaryReport();

                report += "\n\n" +
                    "Individual changes report:\n" +
                    "----------------------------------------------------\n" +
                    MakeIndividualChangesReport();

                return report;
            }

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
        public static BulkCopyPropertiesResult BulkCopyProperties<T>(this T objFrom, T objTo, bool inherit = true)
        {
            // Get all public properties we're considering to check.
            var allProperties = objFrom.GetType().GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    (inherit ? 0 : BindingFlags.DeclaredOnly)
                ).ToList();

            // Get all public properties with the [BulkCopy] attribute.
            var copyList = allProperties.Where(x => x.IsDefined(typeof(BulkCopyAttribute))).ToList();
            var resultList = new List<BulkCopyPropertyResult>();
            foreach (var property in copyList)
            {
                var oldValue = property.GetValue(objTo);
                property.SetValue(objTo, property.GetValue(objFrom));
                var newValue = property.GetValue(objTo);

                resultList.Add(new BulkCopyPropertyResult(property, oldValue, newValue));
            }

            // Get all public properties with the [BulkCopy] attribute.
            var copyContentsList = allProperties.Where(x => x.IsDefined(typeof(BulkCopyRecurseAttribute))).ToList();
            var subResultList = new List<BulkCopyPropertyResult>();
            foreach (var property in copyContentsList)
            {
                var valueFrom = property.GetValue(objFrom);
                var valueTo = property.GetValue(objTo);

                // TODO: how to bundle this into the result??
                // TODO: This should be handled automatically and the results should go to the same collection
                if (typeof(IEnumerable<object>).IsAssignableFrom(valueFrom.GetType()))
                {
                    BulkCopyCollectionProperties(valueFrom as IEnumerable<object>, valueTo as IEnumerable<object>, inherit);
                }
                else
                {
                    BulkCopyProperties(valueFrom, valueTo, inherit);
                }
            }

            return new BulkCopyPropertiesResult(resultList);
        }

        /// <summary>
        /// Copies all properties of type T in an IEnumerable<T> tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <typeparam name="T">The type whose properties should be copied.</typeparam>
        /// <param name="listFrom">The object whose properties should be copied from.</param>
        /// <param name="listTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A report with the number of rows affected, unaffected, and each row's individual properties changed.</returns>
        public static BulkCopyCollectionResult<T> BulkCopyCollectionProperties<T>(this IEnumerable<T> listFrom, IEnumerable<T> listTo, bool inherit = true) where T : class
        {
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
    }
}
