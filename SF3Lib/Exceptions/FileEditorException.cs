using System;

namespace SF3.Exceptions {
    public class FileEditorException : Exception {
        public FileEditorException() {
        }

        public FileEditorException(string message) : base(message) {
        }
    }
}
