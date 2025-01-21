using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public interface ITableView : IView {
        /// <summary>
        /// The table that is to be edited.
        /// </summary>
        ITable Table { get; }

        /// <summary>
        /// The name getter context used to fetch named values for viewing and editing.
        /// </summary>
        INameGetterContext NameGetterContext { get; }

        /// <summary>
        /// The ObjectListView control created during Create().
        /// </summary>
        ObjectListView OLVControl { get; }
    }
}
