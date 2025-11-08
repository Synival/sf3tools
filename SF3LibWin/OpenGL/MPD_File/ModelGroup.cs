using System;
using CommonLib;

namespace SF3.Win.OpenGL.MPD_File {
    public class ModelGroup : IDisposable {
        public ModelGroup(
            QuadModel solidTexturedModel,
            QuadModel solidUntexturedModel,
            QuadModel semiTransparentTexturedModel,
            QuadModel semiTransparentUntexturedModel,
            QuadModel hideModel
        ) {
            SolidTexturedModel             = solidTexturedModel;
            SolidUntexturedModel           = solidUntexturedModel;
            SemiTransparentTexturedModel   = semiTransparentTexturedModel;
            SemiTransparentUntexturedModel = semiTransparentUntexturedModel;
            HideModel                      = hideModel;

            if (solidTexturedModel != null)
                Models.Add(solidTexturedModel);
            if (solidUntexturedModel != null)
                Models.Add(solidUntexturedModel);
            if (semiTransparentTexturedModel != null)
                Models.Add(semiTransparentTexturedModel);
            if (semiTransparentUntexturedModel != null)
                Models.Add(semiTransparentUntexturedModel);
            if (hideModel != null)
                Models.Add(hideModel);
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
        public QuadModel SolidUntexturedModel { get; }
        public QuadModel SemiTransparentTexturedModel { get; }
        public QuadModel SemiTransparentUntexturedModel { get; }
        public QuadModel HideModel { get; }

        public DisposableList<QuadModel> Models { get; } = new DisposableList<QuadModel>();
    }
}
