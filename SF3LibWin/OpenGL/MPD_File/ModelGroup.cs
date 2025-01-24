﻿using System;
using CommonLib;

namespace SF3.Win.OpenGL.MPD_File {
    public class ModelGroup : IDisposable {
        public ModelGroup(QuadModel solidTexturedModel, QuadModel semiTransparentTexturedModel) {
            SolidTexturedModel           = solidTexturedModel;
            SemiTransparentTexturedModel = semiTransparentTexturedModel;

            if (solidTexturedModel != null)
                Models.Add(solidTexturedModel);
            if (semiTransparentTexturedModel != null)
                Models.Add(semiTransparentTexturedModel);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing)
                Models.Dispose();
            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ModelGroup() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public QuadModel SolidTexturedModel { get; }
        public QuadModel SemiTransparentTexturedModel { get; }

        public DisposableList<QuadModel> Models { get; } = new DisposableList<QuadModel>();
    }
}
