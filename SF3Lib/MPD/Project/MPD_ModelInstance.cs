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
            ID        = original.ID;
            ModelID   = original.ModelID;
            PositionX = original.PositionX;
            PositionY = original.PositionY;
            PositionZ = original.PositionZ;
            AngleX    = original.AngleX;
            AngleY    = original.AngleY;
            AngleZ    = original.AngleZ;
            ScaleX    = original.ScaleX;
            ScaleY    = original.ScaleY;
            ScaleZ    = original.ScaleZ;
            Tag       = original.Tag;
            Flags     = original.Flags;
            LevelsOfDetail = original.LevelsOfDetail;
        }

        public static MPD_ModelInstance FromJToken(JToken token, IMPD_ModelCollection collection) => new MPD_ModelInstance(token, collection);
        private MPD_ModelInstance(JToken token, IMPD_ModelCollection collection) {
            Collection = collection;

            var jObject = (JObject) token;

            ID        =    (int) jObject["ID"];
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

        public IMPD_ModelLoD GetModel(int lod)
            => Collection.GetModel(ModelID, lod);

        public IMPD_ModelCollection Collection { get; set; }
        public int ID { get; set; }
        public int ModelID { get; set; }
        public short PositionX { get; set; }
        public short PositionY { get; set; }
        public short PositionZ { get; set; }

        private float _angleX = 0;
        public float AngleX {
            get => _angleX;
            set {
                if (_angleX != value) {
                    _angleX = value;
                    _boundingCube = null;
                }
            }
        }

        private float _angleY = 0;
        public float AngleY {
            get => _angleY;
            set {
                if (_angleY != value) {
                    _angleY = value;
                    _boundingCube = null;
                }
            }
        }

        private float _angleZ = 0;
        public float AngleZ {
            get => _angleZ;
            set {
                if (_angleZ != value) {
                    _angleZ = value;
                    _boundingCube = null;
                }
            }
        }

        private float _scaleX = 0;
        public float ScaleX {
            get => _scaleX;
            set {
                if (_scaleX != value) {
                    _scaleX = value;
                    _boundingCube = null;
                }
            }
        }

        private float _scaleY = 0;
        public float ScaleY {
            get => _scaleY;
            set {
                if (_scaleY != value) {
                    _scaleY = value;
                    _boundingCube = null;
                }
            }
        }

        private float _scaleZ = 0;
        public float ScaleZ {
            get => _scaleZ;
            set {
                if (_scaleZ != value) {
                    _scaleZ = value;
                    _boundingCube = null;
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

        private BoundingCube? _boundingCube = null;
        public BoundingCube BoundingCube {
            get {
                if (!_boundingCube.HasValue) {
                    _boundingCube = GetModel(0).Vertices.AsArray()
                        .CreateBoundingCube()
                        .ToVECTORs()
                        .Scale(ScaleX, ScaleY, ScaleZ)
                        .RotateXYZ(AngleX, AngleY, AngleZ)
                        .CreateBoundingCube();
                }
                return _boundingCube.Value;
            }
        }
    }
}
