using System.Linq;
using System.Numerics;
using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using static CommonLib.Extensions.VECTOR_Extensions;

namespace CommonLib.SGL {
    public abstract class SGL_ModelInstanceBase : ISGL_ModelInstance {
        public SGL_ModelInstanceBase() {
        }

        public SGL_ModelInstanceBase(ISGL_ModelInstance original) {
            ModelCollectionID = original.ModelCollectionID;
            ModelID           = original.ModelID;
            ModelInstanceID   = original.ModelInstanceID;

            _positionX = original.PositionX;
            _positionY = original.PositionY;
            _positionZ = original.PositionZ;
            _angleX    = original.AngleX;
            _angleY    = original.AngleY;
            _angleZ    = original.AngleZ;
            _scaleX    = original.ScaleX;
            _scaleY    = original.ScaleY;
            _scaleZ    = original.ScaleZ;
            _matrix    = original.Matrix;

            LevelsOfDetail = original.LevelsOfDetail;
        }

        protected SGL_ModelInstanceBase(JToken token) {
            var jObject = (JObject) token;

            ModelCollectionID = (int?) jObject.GetValueIfExists("ModelCollectionID", t => (JValue) t) ?? 0;
            ModelID   =    (int) jObject["ModelID"];
            ModelInstanceID = (int) jObject["ID"];
            PositionX =  (short) jObject["PositionX"];
            PositionY =  (short) jObject["PositionY"];
            PositionZ =  (short) jObject["PositionZ"];
            AngleX    =  (float) jObject["AngleX"];
            AngleY    =  (float) jObject["AngleY"];
            AngleZ    =  (float) jObject["AngleZ"];
            ScaleX    =  (float) jObject["ScaleX"];
            ScaleY    =  (float) jObject["ScaleY"];
            ScaleZ    =  (float) jObject["ScaleZ"];
            LevelsOfDetail = (int?) jObject.GetValueIfExists("LevelsOfDetail", t => (JValue) t) ?? 1;
        }

        public abstract ISGL_Model GetModel(int lod);

        public virtual int ModelCollectionID { get; set; }
        public virtual int ModelID { get; set; }
        public virtual int ModelInstanceID { get; set; }

        private short _positionX = 0;
        public short PositionX {
            get => _positionX;
            set {
                if (_positionX != value) {
                    _positionX = value;
                    _boundingBox = null;
                }
            }
        }

        private short _positionY = 0;
        public short PositionY {
            get => _positionY;
            set {
                if (_positionY != value) {
                    _positionY = value;
                    _boundingBox = null;
                }
            }
        }

        private short _positionZ = 0;
        public short PositionZ {
            get => _positionZ;
            set {
                if (_positionZ != value) {
                    _positionZ = value;
                    _boundingBox = null;
                }
            }
        }

        private float _angleX = 0;
        public float AngleX {
            get => _angleX;
            set {
                if (_angleX != value) {
                    _angleX = value;
                    _boundingBox = null;
                }
            }
        }

        private float _angleY = 0;
        public float AngleY {
            get => _angleY;
            set {
                if (_angleY != value) {
                    _angleY = value;
                    _boundingBox = null;
                }
            }
        }

        private float _angleZ = 0;
        public float AngleZ {
            get => _angleZ;
            set {
                if (_angleZ != value) {
                    _angleZ = value;
                    _boundingBox = null;
                }
            }
        }

        private float _scaleX = 1.0f;
        public float ScaleX {
            get => _scaleX;
            set {
                if (_scaleX != value) {
                    _scaleX = value;
                    _boundingBox = null;
                }
            }
        }

        private float _scaleY = 1.0f;
        public float ScaleY {
            get => _scaleY;
            set {
                if (_scaleY != value) {
                    _scaleY = value;
                    _boundingBox = null;
                }
            }
        }

        private float _scaleZ = 1.0f;
        public float ScaleZ {
            get => _scaleZ;
            set {
                if (_scaleZ != value) {
                    _scaleZ = value;
                    _boundingBox = null;
                }
            }
        }

        private Matrix4x4? _matrix = null;
        public Matrix4x4? Matrix {
            get => _matrix;
            set {
                if (_matrix != value) {
                    _matrix = value;
                    _boundingBox = null;
                }
            }
        }

        public virtual bool AlwaysFacesCamera { get; set; } = false;
        public int LevelsOfDetail { get; set; }

        private BoundingBox? _boundingBox = null;
        public BoundingBox BoundingBox {
            get {
                if (!_boundingBox.HasValue) {
                    _boundingBox = GetModel(0).Vertices.ToArray()
                        .CreateBoundingBox()
                        .ToVECTORs()
                        .Scale(ScaleX, ScaleY, ScaleZ)
                        .RotateXYZ(AngleX, AngleY, AngleZ)
                        // TODO: Apply matrix if exists!
                        .CreateBoundingBox();
                }
                return _boundingBox.Value;
            }
        }

        public float? ForceTransparency { get; set; }
    }
}
