using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.Values {
    public class FileIndexValueResourceInfo : NamedValueFromResourceForScenariosInfo {
        public FileIndexValueResourceInfo() : base("FileIndexes.xml") {
        }

        public override string FormatString => "X4";
    }

    /// <summary>
    /// Named value for file index that can be bound to an ObjectListView.
    /// </summary>
    public class FileIndexValue : NamedValueFromResourceForScenarios<FileIndexValueResourceInfo> {
        public FileIndexValue(ScenarioType scenario, int value) : base(scenario, value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new FileIndexValue(Scenario, value);
    }
}
