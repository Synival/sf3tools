﻿using System.Collections.Generic;

namespace CommonLib {
    /// <summary>
    /// Interface for information about a named value type.
    /// </summary>
    public interface INamedValueInfo {
        int MinValue { get; }
        int MaxValue { get; }
        string FormatString { get; }
        Dictionary<int, string> Values { get; }
        Dictionary<int, string> NewComboBoxValues { get; }
    }
}