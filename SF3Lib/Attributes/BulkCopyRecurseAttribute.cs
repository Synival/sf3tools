using System;

namespace SF3.Attributes {
    /// <summary>
    /// Used to indicate that BulkCopyProperties() should peek inside this property and recursively copy its contents.
    /// </summary>
    public class BulkCopyRecurseAttribute : Attribute {
    }
}
