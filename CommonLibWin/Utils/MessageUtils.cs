using System.Reflection;
using System.Windows.Forms;

namespace CommonLib.Win.Utils {
    public static class MessageUtils {
        private static string _errorMessageTitle = null;

        /// <summary>
        /// Initializes the message title if not initialized and returns it.
        /// </summary>
        /// <returns>A string for the message title to use.</returns>
        public static string MessageTitle() {
            if (_errorMessageTitle == null) {
                var assembly = Assembly.GetEntryAssembly();
                _errorMessageTitle = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? assembly.GetName().Name;
            }
            return _errorMessageTitle;
        }

        /// <summary>
        /// Shows an error MessageBox with the correct title, buttons, and icon.
        /// </summary>
        /// <param name="message">The error messsage to display.</param>
        public static void ErrorMessage(string message)
            => MessageBox.Show(message, MessageTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);

        /// <summary>
        /// Shows a warning MessageBox with the correct title, buttons, and icon.
        /// </summary>
        /// <param name="message">The warning messsage to display.</param>
        public static void WarningMessage(string message)
            => MessageBox.Show(message, MessageTitle(), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        /// <summary>
        /// Shows an info MessageBox with the correct title, buttons, and icon.
        /// </summary>
        /// <param name="message">The info messsage to display.</param>
        public static void InfoMessage(string message)
            => MessageBox.Show(message, MessageTitle(), MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
