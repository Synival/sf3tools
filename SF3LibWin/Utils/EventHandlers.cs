using System.Windows.Forms;

namespace SF3.Win.Utils {
    public static class EventHandlers {
        public delegate void CmdKeyEventHandler(object sender, ref Message msg, Keys keyData, ref bool wasProcessed);
        public delegate void FrameTickEventHandler(object sender, float deltaInMs);
    }
}
