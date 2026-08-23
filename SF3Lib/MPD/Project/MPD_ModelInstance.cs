using System.Linq;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;
using static CommonLib.Extensions.VECTOR_Extensions;

namespace SF3.MPD.Project {
    public class MPD_ModelInstance : IMPD_ModelInstance {
        public MPD_ModelInstance() {
            OnlyVisibleFromDirection = ModelDirectionType.Unset;
        }

        public MPD_ModelInstance(IMPD_ModelInstance original, IMPD_ModelCollection newCollection) {
            Collection = newCollection;
            ModelID    = original.ModelID;
            ModelInstanceID = original.ModelInstanceID;
            _positionX = original.PositionX;
            _positionY = original.PositionY;
            _positionZ = original.PositionZ;
            _angleX    = original.AngleX;
            _angleY    = original.AngleY;
            _angleZ    = original.AngleZ;
            _scaleX    = original.ScaleX;
            _scaleY    = original.ScaleY;
            _scaleZ    = original.ScaleZ;
            Tag        = original.Tag;
            Flags      = original.Flags;
            LevelsOfDetail = original.LevelsOfDetail;
        }

        public static MPD_ModelInstance FromJToken(JToken token, IMPD_ModelCollection collection) => new MPD_ModelInstance(token, collection);
        private MPD_ModelInstance(JToken token, IMPD_ModelCollection collection) {
            Collection = collection;

            var jObject = (JObject) token;

            ModelInstanceID = (int) jObject["ID"];
            ModelID   =    (int) jObject["ModelID"];
            PositionX =  (short) jObject["PositionX"];
            PositionY =  (short) jObject["PositionY"];
            PositionZ =  (short) jObject["PositionZ"];
            AngleX    =  (float) jObject["AngleX"];
            AngleY    =  (float) jObject["AngleY"];
            AngleZ    =  (float) jObject["AngleZ"];
            ScaleX    =  (float) jObject["ScaleX"];
            ScaleY    =  (float) jObject["ScaleY"];
            ScaleZ    =  (float) jObject["ScaleZ"];

            if (!Collection.IsHeaderModelCollection()) {
                Tag   = (ushort) jObject["Tag"];
                Flags = (ushort) jObject["Flags"];
                LevelsOfDetail = (int) jObject["LevelsOfDetail"];
            }
        }

        ISGL_Model ISGL_ModelInstance.GetModel(int lod) => GetModel(lod);
        public IMPD_ModelLoD GetModel(int lod)
            => Collection.GetModel(ModelID, lod);

        public IMPD_ModelCollection Collection { get; set; }

        public int ModelCollectionID => (int) Collection.Collection;
        public int ModelID { get; set; }
        public int ModelInstanceID { get; set; }

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

        private float _scaleX = 0;
        public float ScaleX {
            get => _scaleX;
            set {
                if (_scaleX != value) {
                    _scaleX = value;
                    _boundingBox = null;
                }
            }
        }

        private float _scaleY = 0;
        public float ScaleY {
            get => _scaleY;
            set {
                if (_scaleY != value) {
                    _scaleY = value;
                    _boundingBox = null;
                }
            }
        }

        private float _scaleZ = 0;
        public float ScaleZ {
            get => _scaleZ;
            set {
                if (_scaleZ != value) {
                    _scaleZ = value;
                    _boundingBox = null;
                }
            }
        }

        public ushort Tag { get; set; }
        public ushort Flags { get; set; }

        public bool AlwaysFacesCamera {
            get => (Flags & 0x08) == 0x08;
            set => Flags = (ushort) (Flags & ~0x08 | (value ? 0x08 : 0x00));
        }

        public ModelDirectionType OnlyVisibleFromDirection {
            get => (Flags & 0x10) == 0x10 ? (ModelDirectionType) (Flags & 0x07) : ModelDirectionType.Unset;
            set {
                Flags = (value < 0x00 || (ushort) value > 0x07)
                    ? (ushort) (Flags & ~0x17)
                    : (ushort) (Flags | 0x10 | ((ushort) value & 0x07));
            }
        }

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
                        .CreateBoundingBox();
                }
                return _boundingBox.Value;
            }
        }
    }
}
