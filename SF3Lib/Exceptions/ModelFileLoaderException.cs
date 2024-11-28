using System;

namespace SF3.Exceptions {
    public class ModelFileLoaderException : Exception {
        public ModelFileLoaderException() {
        }

        public ModelFileLoaderException(string message) : base(message) {
        }
    }
}
