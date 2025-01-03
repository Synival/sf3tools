using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonLib.Attributes;
using CommonLib.NamedValues;

namespace CommonLib.Extensions {
    public static partial class ObjectExtensions {
        private static string BulkReportPropertyTypeString(PropertyInfo property)
            => BulkReportTypeString(property.PropertyType);

        private static string BulkReportTypeString(Type type)
            // This is useful for debugging, but a bit overwhelming for most folk!
#if true
            => "";
#else
            => " (" + type.Name + ")";
#endif

        private static string GetBulkCopyRowName(this object obj, PropertyInfo property) {
            var rowNameProperties = obj
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.IsDefined(typeof(BulkCopyRowNameAttribute)))
                .Select(x => x.GetValue(obj).ToString())
                .ToList();
            var bulkCopyRowName = rowNameProperties.Any() ? string.Join(" / ", rowNameProperties) : null;

            if (property == null && bulkCopyRowName == null)
                return obj.GetType().Name;
            else if (property != null && bulkCopyRowName != null)
                return property.Name + " / " + bulkCopyRowName + " " + BulkReportPropertyTypeString(property);
            else if (property != null)
                return property.Name + BulkReportPropertyTypeString(property);
            else
                return bulkCopyRowName + BulkReportTypeString(obj.GetType());
        }

        /// <summary>
        /// Any kind of result from any kind of BulkCopy() operation.
        /// </summary>
        public interface IBulkCopyResult {
            /// <summary>
            /// Nested results from any objects in a collection copied or objects copied recursively.
            /// </summary>
            IEnumerable<IBulkCopyResult> ChildResults { get; }

            /// <summary>
            /// Produces a report for itself as well as all children.
            /// </summary>
            /// <returns>A string separated by newlines without a trailing newline.</returns>
            string MakeFullReport(INameGetterContext nameContext);

            /// <summary>
            /// Produces a report for itself.
            /// </summary>
            /// <returns></returns>
            string MakeSummaryReport(INameGetterContext nameContext);

            /// <summary>
            /// 'True' if any changes were made.
            /// </summary>
            bool Changed { get; }

            /// <summary>
            /// Returns the total number of changes.
            /// </summary>
            int ChangeCount { get; }
        }

        /// <summary>
        /// Any kind of result from any kind of BulkCopy() operation.
        /// </summary>
        public abstract class BulkCopyResult : IBulkCopyResult {
            public BulkCopyResult(IEnumerable<IBulkCopyResult> childResults) {
                ChildResults = childResults;
            }

            public abstract string MakeFullReport(INameGetterContext nameContext);
            public abstract string MakeSummaryReport(INameGetterContext nameContext);

            public IEnumerable<IBulkCopyResult> ChildResults { get; }
            public abstract bool Changed { get; }
            public abstract int ChangeCount { get; }
        }

        /// <summary>
        /// Result for individual properties affected by BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyPropertyResult : BulkCopyResult {
            public BulkCopyPropertyResult(object objFrom, object objTo, PropertyInfo property, object oldValue, object newValue)
            : base(null) {
                ObjFrom = objFrom;
                ObjTo = objTo;
                Property = property;
                OldValue = oldValue;
                NewValue = newValue;
                Changed = !NewValue.Equals(OldValue);
                ChangeCount = Changed ? 1 : 0;
                Name = NewValue.GetBulkCopyRowName(Property);
            }

            public override string MakeFullReport(INameGetterContext nameContext) => MakeSummaryReport(nameContext);

            public override string MakeSummaryReport(INameGetterContext nameContext) {
                return !Changed ? "" : Name + ": " +
                    (ObjTo.GetPropertyValueName(Property, nameContext, OldValue) ?? OldValue.ToStringHex()) + " => " +
                    (ObjTo.GetPropertyValueName(Property, nameContext, NewValue) ?? NewValue.ToStringHex()) + "\n";
            }

            /// <summary>
            /// The property affected.
            /// </summary>
            public PropertyInfo Property { get; }

            /// <summary>
            /// The object from which the value was copied.
            /// </summary>
            public object ObjFrom { get; }

            /// <summary>
            /// The object to which the value was copied.
            /// </summary>
            public object ObjTo { get; }

            /// <summary>
            /// The old value of the property before the copy.
            /// </summary>
            public object OldValue { get; }

            /// <summary>
            /// The new value of the property after the copy.
            /// </summary>
            public object NewValue { get; }

            /// <summary>
            /// Name of the value changed.
            /// </summary>
            public string Name { get; }

            public override bool Changed { get; }
            public override int ChangeCount { get; }
        }

        /// <summary>
        /// Collection of BulkCopyPropertyResult's reported by BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyPropertiesResult : BulkCopyResult {
            public BulkCopyPropertiesResult(object obj, IEnumerable<IBulkCopyResult> childResults, PropertyInfo property = null)
            : base(childResults) {
                Object = obj;
                ChangeCount = childResults?.Count(x => x.Changed) ?? 0;
                Changed = ChangeCount > 0;
                Name = obj.GetBulkCopyRowName(property);
            }

            public override string MakeFullReport(INameGetterContext nameContext) {
                var report = string.Join("", ChildResults.Select(x => x.MakeFullReport(nameContext)));
                return (report == "") ? "" : Name + ":\n" + report.Indent("  ");
            }

            public override string MakeSummaryReport(INameGetterContext nameContext) {
                var report = string.Join("", ChildResults.Select(x => x.MakeSummaryReport(nameContext)));
                return (report == "") ? "" : Name + ":\n" + report.Indent("  ");
            }

            /// <summary>
            /// The object whose properties were bulk-copied.
            /// </summary>
            public object Object { get; }

            /// <summary>
            /// The name of the object bulk-copied.
            /// </summary>
            public string Name { get; }

            public override bool Changed { get; }
            public override int ChangeCount { get; }
        }

        /// <summary>
        /// Result for an individual row in BulkCopyCollectionResult.
        /// </summary>
        public class BulkCopyCollectionRowResult : IBulkCopyResult {
            /// <summary>
            /// Bulk copy result for a row that was or was not copied, depending on whether 'copyResult' is 'null'.
            /// </summary>
            /// <param name="index">Index of the row in 'listFrom'.</param>
            /// <param name="copyResult">Result of the row copied. Can be 'null' to indicate the row wasn't copied.</param>
            public BulkCopyCollectionRowResult(string key, object rowFrom, object rowTo, BulkCopyPropertiesResult copyResult) {
                Key = key;
                RowFrom = rowFrom;
                RowTo = rowTo;
                Copied = rowTo != null && copyResult != null;
                CopyResult = copyResult;
                Changed = Copied && (copyResult?.Changed ?? false);
                ChangeCount = Changed ? copyResult.ChangeCount : 0;
            }

            public string MakeFullReport(INameGetterContext nameContext)
                => CopyResult?.MakeFullReport(nameContext) ?? "";

            public string MakeSummaryReport(INameGetterContext nameContext)
                => CopyResult?.MakeSummaryReport(nameContext) ?? "";

            /// <summary>
            /// The key/index of the row in the 'listFrom' collection.
            /// </summary>
            public string Key { get; }

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

            public IEnumerable<IBulkCopyResult> ChildResults => CopyResult?.ChildResults;

            public bool Changed { get; }

            public int ChangeCount { get; }
        }

        /// <summary>
        /// Result for BulkCopyProperties() functions.
        /// </summary>
        public class BulkCopyCollectionResult : BulkCopyResult {
            public BulkCopyCollectionResult(IEnumerable collection, IEnumerable<IBulkCopyResult> inputResults, int listOutRowsIgnored, PropertyInfo property = null)
            : base(inputResults) {
                Collection = collection;
                InputRows = inputResults
                    .Where(x => typeof(BulkCopyCollectionRowResult).IsAssignableFrom(x.GetType()))
                    .Cast<BulkCopyCollectionRowResult>()
                    .ToList();
                InRowsSkipped = InputRows.Count(x => !x.Copied);
                OutRowsSkipped = listOutRowsIgnored;
                RowsCopied = InputRows.Count(x => x.Copied);
                ChangeCount = inputResults?.Count(x => x.Changed) ?? 0;
                Changed = ChangeCount > 0;
                Name = collection.GetBulkCopyRowName(property);
            }

            public override string MakeFullReport(INameGetterContext nameContext) {
                var report =
                    "Summary:\n" +
                    MakeSummaryReport(nameContext, true).Indent("  ");

                var individualChangesReport = MakeIndividualChangesReport(nameContext, true);
                report +=
                    "Changes:\n" +
                    ((individualChangesReport == "") ? "  (none)\n" : individualChangesReport.Indent("  "));

                return report == "" ? "" : Name + ":\n" + report.Indent("   ");
            }

            public override string MakeSummaryReport(INameGetterContext nameContext) {
                var report = MakeSummaryReport(nameContext, false);
                return report == "" ? "" : Name + ":\n" + report.Indent("   ");
            }

            public string MakeSummaryReport(INameGetterContext nameContext, bool inFullReport) {
                var report =
                    (inFullReport ? ("Rows copied: " + RowsCopied + "\n") : "") +
                    ((inFullReport || InRowsSkipped != 0) ? ("Input rows skipped: " + InRowsSkipped + "\n") : "") +
                    ((inFullReport || OutRowsSkipped != 0) ? ("Target rows unaffected: " + OutRowsSkipped + "\n") : "");

                var rowsWithChanges = InputRows.Where(x => x.Copied && x.CopyResult.Changed).ToList();
                report += "Rows changed: " + rowsWithChanges.Count + "\n";

                var cellsChanged = rowsWithChanges.Sum(x => x.CopyResult.ChangeCount);
                report += "Cells changed: " + cellsChanged + "\n";

                // We probably will never have this, but just in case...
                var otherRows = ChildResults.Except(InputRows).ToList();
                if (otherRows.Any())
                    report += string.Join("", otherRows.Select(x => MakeSummaryReport(nameContext).Indent("  ")).ToList());

                return report;
            }

            /// <summary>
            /// Produces a detailed report of all changes in the list.
            /// </summary>
            /// <returns>A multiline string with a trailing "\n".</returns>
            public string MakeIndividualChangesReport(INameGetterContext nameContext, bool fullReport = false) {
                var report = "";

                foreach (var row in InputRows) {
                    var rowReport = fullReport ? row.MakeFullReport(nameContext) : row.MakeSummaryReport(nameContext);
                    if (rowReport == "")
                        continue;

                    // Other the other code to show array/dictionary keys.
#if true
                    report += rowReport.Indent("  ").TrimStart();
#else
                    report += "[" + row.Key + "]" + rowReport.Indent("  ").TrimStart();
#endif
                }

                return report;
            }

            /// <summary>
            /// The collection iterated over.
            /// </summary>
            public IEnumerable Collection { get; }

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
            /// The name of the collection copied.
            /// </summary>
            public string Name { get; }

            public override bool Changed { get; }
            public override int ChangeCount { get; }
        }

        /// <summary>
        /// Copies all properties tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <param name="objFrom">The object whose properties should be copied from.</param>
        /// <param name="objTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A list of all properties considered and the result.</returns>
        public static BulkCopyPropertiesResult BulkCopyProperties(this object objFrom, object objTo, bool inherit = true, PropertyInfo property = null) {
            // Get all public properties we're considering to check.
            var allProperties = objFrom.GetType().GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    (inherit ? 0 : BindingFlags.DeclaredOnly)
                ).ToList();

            // Get all public properties with the [BulkCopy] attribute.
            var copyList = allProperties.Where(x => x.IsDefined(typeof(BulkCopyAttribute))).ToList();
            var resultList = new List<IBulkCopyResult>();
            foreach (var copyProperty in copyList) {
                var oldValue = copyProperty.GetValue(objTo);
                copyProperty.SetValue(objTo, copyProperty.GetValue(objFrom));
                var newValue = copyProperty.GetValue(objTo);

                resultList.Add(new BulkCopyPropertyResult(objFrom, objTo, copyProperty, oldValue, newValue));
            }

            // Get all public properties with the [BulkCopy] attribute.
            var copyContentsList = allProperties.Where(x => x.IsDefined(typeof(BulkCopyRecurseAttribute))).ToList();
            var subResultList = new List<BulkCopyPropertyResult>();
            foreach (var copyProperty in copyContentsList) {
                var valueFrom = copyProperty.GetValue(objFrom);
                var valueTo = copyProperty.GetValue(objTo);

                // Don't recurse through objects that are unassigned, on either end.
                // We have no idea how to instantiate them anyway.
                // TODO: at least log this!
                if (valueFrom == null || valueTo == null)
                    continue;

                var type = valueFrom.GetType();
                if (typeof(IDictionary).IsAssignableFrom(type))
                    resultList.Add(BulkCopyCollectionProperties(valueFrom as IDictionary, valueTo as IDictionary, inherit, copyProperty));
                if (typeof(IEnumerable<object>).IsAssignableFrom(type))
                    resultList.Add(BulkCopyCollectionProperties(valueFrom as IEnumerable<object>, valueTo as IEnumerable<object>, inherit, copyProperty));
                else
                    resultList.Add(BulkCopyProperties(valueFrom, valueTo, inherit, copyProperty));
            }

            return new BulkCopyPropertiesResult(objFrom, resultList, property);
        }

        /// <summary>
        /// Copies all properties in an IEnumerable tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <param name="listFrom">The object whose properties should be copied from.</param>
        /// <param name="listTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A report with the number of rows affected, unaffected, and each row's individual properties changed.</returns>
        public static BulkCopyCollectionResult BulkCopyCollectionProperties(this IEnumerable<object> listFrom, IEnumerable<object> listTo, bool inherit = true, PropertyInfo property = null) {
            var arrayFrom = listFrom.ToArray();
            var arrayTo = listTo.ToArray();

            var inputRowReports = new List<BulkCopyCollectionRowResult>();
            for (var i = 0; i < arrayFrom.Length; i++) {
                var rowFrom = arrayFrom[i];
                var rowTo = (i < arrayTo.Length) ? arrayTo[i] : null;
                if (rowTo != null)
                    inputRowReports.Add(new BulkCopyCollectionRowResult(i.ToString(), rowFrom, rowTo, BulkCopyProperties(rowFrom, rowTo, inherit)));
            }

            return new BulkCopyCollectionResult(listFrom, inputRowReports, Math.Max(arrayTo.Length - arrayFrom.Length, 0), property);
        }

        /// <summary>
        /// Copies all properties in an IDictionary tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <param name="dictFrom">The object whose properties should be copied from.</param>
        /// <param name="dictTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A report with the number of rows affected, unaffected, and each row's individual properties changed.</returns>
        public static BulkCopyCollectionResult BulkCopyCollectionProperties(this IDictionary dictFrom, IDictionary dictTo, bool inherit = true, PropertyInfo property = null) {
            var inputRowReports = new List<BulkCopyCollectionRowResult>();
            foreach (var kvObj in dictFrom) {
                var kv = (DictionaryEntry) kvObj;
                var dictValueFrom = kv.Value;
                var dictValueTo = dictTo.Contains(kv.Key) ? dictTo[kv.Key] : null;
                if (dictValueTo != null)
                    inputRowReports.Add(new BulkCopyCollectionRowResult(kv.Key.ToString(), dictValueFrom, dictValueFrom, BulkCopyProperties(dictValueFrom, dictValueTo, inherit)));
            }
            return new BulkCopyCollectionResult(dictFrom, inputRowReports, Math.Max(dictTo.Count - dictFrom.Count, 0), property);
        }
    }
}
