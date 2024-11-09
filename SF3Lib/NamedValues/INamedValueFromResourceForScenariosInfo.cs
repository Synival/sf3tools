using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.NamedValues {
    /// <summary>
    /// Interface for named value info from a resource that is specific to each scenario.
    /// </summary>
    public interface INamedValueFromResourceForScenariosInfo {
        string ResourceName { get; }
        int MinValue { get; }
        int MaxValue { get; }
        string FormatString { get; }
        Dictionary<ScenarioType, INamedValueInfo> Info { get; }
    };
}
