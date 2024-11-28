using System.IO;
using SF3.Models.Files;
using SF3.RawData;

namespace SF3.ModelLoaders {
    public static class ModelFileLoaderDelegates {

        public delegate IBaseFile ModelFileLoaderCreateModelDelegate(IModelFileLoader loader);
        public delegate IRawData ModelFileLoaderCreateRawEditorDelegate(IModelFileLoader loader, string filename, Stream stream);
    }
}
