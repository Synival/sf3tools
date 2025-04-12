using System.IO;
using SF3.ByteData;
using SF3.Models.Files;

namespace SF3.ModelLoaders {
    public static class ModelFileLoaderDelegates {
        public delegate IBaseFile ModelFileLoaderCreateModelDelegate(IModelFileLoader loader);
        public delegate IByteData ModelFileLoaderCreateByteDataDelegate(IModelFileLoader loader, string filename, string fileDialogFilter, Stream stream);
    }
}
