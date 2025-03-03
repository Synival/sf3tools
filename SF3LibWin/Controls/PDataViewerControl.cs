using System;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Win.Controls {
    public partial class PDataViewerControl : GLControl {
        public PDataViewerControl() {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            MakeCurrent();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquationSeparate(BlendEquationMode.FuncAdd, BlendEquationMode.Max);
        }

        private PDataModel _pdata = null;

        public PDataModel PData {
            get => _pdata;
            set {
                if (_pdata != value) {
                    _pdata = value;
                    Invalidate();
                }
            }
        }
    }
}
