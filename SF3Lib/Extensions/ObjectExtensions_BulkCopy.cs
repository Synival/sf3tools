using SF3.Attributes;
using System;
using System.Collections;
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
        /// Any kind of result from any kind of BulkCopy() operation.
        /// </summary>
        public interface IBulkCopyResult
        {
            /// <summary>
            /// Nested results from any objects in a collection copied or objects copied recursively.
            /// </summary>
            IEnumerable<IBulkCopyResult> ChildResults { get; }
        }

        /// <summary>
        /// Any kind of result from any kind of BulkCopy() operation.
        /// </summary>
        public class BulkCopyResult : IBulkCopyResult
        {
            public BulkCopyResult(IEnumerable<IBulkCopyResult> childResults)
            {
                ChildResults = childResults;
            }

            public IEnumerable<IBulkCopyResult> ChildResults { get; } = new List<BulkCopyResult>();
        }

        /// <summary>
        /// Result for individual properties affected by BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyPropertyResult : BulkCopyResult
        {
            public BulkCopyPropertyResult(PropertyInfo property, object oldValue, object newValue)
            : base(null)
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
        public class BulkCopyPropertiesResult : BulkCopyResult
        {
            public BulkCopyPropertiesResult(IEnumerable<IBulkCopyResult> childResults)
            : base(childResults)
            {
            }
        }

        /// <summary>
        /// Result for an individual row in BulkCopyCollectionResult.
        /// </summary>
        public class BulkCopyCollectionRowResult : IBulkCopyResult
        {
            /// <summary>
            /// Bulk copy result for a row that was or was not copied, depending on whether 'copyResult' is 'null'.
            /// </summary>
            /// <param name="index">Index of the row in 'listFrom'.</param>
            /// <param name="copyResult">Result of the row copied. Can be 'null' to indicate the row wasn't copied.</param>
            public BulkCopyCollectionRowResult(int index, object rowFrom, object rowTo, BulkCopyPropertiesResult copyResult)
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
            public object RowFrom { get; }

            /// <summary>
            /// The output row.
            /// </summary>
            public object RowTo { get; }

            /// <summary>
            /// 'True' if this row was copied.
            /// </summary>
            public bool Copied { get; }

            /// <summary>
            /// Report for all properties copied. Can be 'null' if this row was not copied (i.e, 'Copied' is 'false').
            /// </summary>
            public BulkCopyPropertiesResult CopyResult { get; }

            public IEnumerable<IBulkCopyResult> ChildResults => (CopyResult != null) ? CopyResult.ChildResults : null;
        }

        /// <summary>
        /// Result for BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyCollectionResult : BulkCopyResult
        {
            public BulkCopyCollectionResult(IEnumerable<IBulkCopyResult> inputResults, int listOutRowsIgnored)
            : base(inputResults)
            {
                InputRows = inputResults
                    .Where(x => typeof(BulkCopyCollectionRowResult).IsAssignableFrom(x.GetType()))
                    .Cast<BulkCopyCollectionRowResult>()
                    .ToList();
                InRowsSkipped = InputRows.Count(x => !x.Copied);
                OutRowsSkipped = listOutRowsIgnored;
                RowsCopied = InputRows.Count(x => x.Copied);
            }

            /// <summary>
            /// All rows attempted to be copied.
            /// </summary>
            public IEnumerable<BulkCopyCollectionRowResult> InputRows { get; }

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
                    "----------------------------------------------------\n" +
                    MakeSummaryReport();

                report += "\n\n" +
                    "Individual changes report:\n" +
                    "----------------------------------------------------\n" +
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

                // TODO: get this working!
/*
                var rowsWithChanges = InputRows.Where(x => x.Copied && x.CopyResult.Properties.Any(y => y.Changed)).ToList();
                report += "Rows changed: " + rowsWithChanges.Count + "\n";

                var cellsChanged = rowsWithChanges.Sum(x => x.CopyResult.Properties.Count(y => y.Changed));
                report += "Cells changed: " + cellsChanged + "\n";
*/

                return report;
            }

            /// <summary>
            /// Produces a detailed report of all changes in the list.
            /// </summary>
            /// <returns>A multiline string with a trailing "\n".</returns>
            public string MakeIndividualChangesReport()
            {
                string report = "";
                // TODO: get this working again!!
/*
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
*/
                return report;
            }
        }

        /// <summary>
        /// Copies all properties tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <param name="objFrom">The object whose properties should be copied from.</param>
        /// <param name="objTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A list of all properties considered and the result.</returns>
        public static BulkCopyPropertiesResult BulkCopyProperties(this object objFrom, object objTo, bool inherit = true)
        {
            // Get all public properties we're considering to check.
            var allProperties = objFrom.GetType().GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    (inherit ? 0 : BindingFlags.DeclaredOnly)
                ).ToList();

            // Get all public properties with the [BulkCopy] attribute.
            var copyList = allProperties.Where(x => x.IsDefined(typeof(BulkCopyAttribute))).ToList();
            var resultList = new List<IBulkCopyResult>();
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

                if (typeof(IEnumerable<object>).IsAssignableFrom(valueFrom.GetType()))
                {
                    resultList.Add(BulkCopyCollectionProperties(valueFrom as IEnumerable<object>, valueTo as IEnumerable<object>, inherit));
                }
                else
                {
                    resultList.Add(BulkCopyProperties(valueFrom, valueTo, inherit));
                }
            }

            return new BulkCopyPropertiesResult(resultList);
        }

        /// <summary>
        /// Copies all properties in an IEnumerable tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <param name="listFrom">The object whose properties should be copied from.</param>
        /// <param name="listTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A report with the number of rows affected, unaffected, and each row's individual properties changed.</returns>
        public static BulkCopyCollectionResult BulkCopyCollectionProperties(this IEnumerable<object> listFrom, IEnumerable<object> listTo, bool inherit = true)
        {
            var arrayFrom = listFrom.ToArray();
            var arrayTo = listTo.ToArray();

            var inputRowReports = new List<BulkCopyCollectionRowResult>();
            for (int i = 0; i < arrayFrom.Length; i++)
            {
                var rowTo = (i < arrayTo.Length) ? arrayTo[i] : null;
                inputRowReports.Add(new BulkCopyCollectionRowResult(i, arrayFrom[i], rowTo,
                    (rowTo != null) ? BulkCopyProperties(arrayFrom[i], rowTo, inherit) : null));
            }

            return new BulkCopyCollectionResult(inputRowReports, Math.Max(arrayTo.Length - arrayFrom.Length, 0));
        }
    }
}
