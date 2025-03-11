using SF3.Models.Structs.X005;

namespace SF3.Models.Files.X005 {
    public interface IX005_File : IScenarioTableFile {
        CameraSettings CameraSettings { get; }
    }
}
