using System.IO;
using SF3.Editors;
using SF3.RawEditors;

namespace SF3.Loaders {
    public static class FileLoaderDelegates {

        public delegate IBaseEditor FileLoaderCreateEditorDelegate(IFileLoader loader);
        public delegate IRawEditor FileLoaderCreateRawEditorDelegate(IFileLoader loader, string filename, Stream stream);
    }
}
