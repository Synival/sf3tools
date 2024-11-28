﻿using System.IO;
using SF3.FileModels;
using SF3.RawEditors;

namespace SF3.ModelLoaders {
    public static class ModelFileLoaderDelegates {

        public delegate IBaseEditor ModelFileLoaderCreateModelDelegate(IModelFileLoader loader);
        public delegate IRawEditor ModelFileLoaderCreateRawEditorDelegate(IModelFileLoader loader, string filename, Stream stream);
    }
}
