using SF3.Types;

namespace SF3.Actors {
    public class ActorScriptCommand {
        public ActorScriptCommand(uint[] data, string comment) {
            Data    = data;
            Comment = comment;
        }

        public uint[] Data { get; }
        public string Comment { get; }

        public ActorCommandType Command => (ActorCommandType) Data[0];
    }
}
