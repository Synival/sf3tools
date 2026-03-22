using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_ModelLoD : IMPD_ModelLoD {
        public MPD_ModelLoD(ISGL_Model model, MPD_CollectionType collection, int modelId, int lod) {
            _actualModel  = model;

            Collection    = collection;
            ModelID       = modelId;
            LevelOfDetail = lod;

            Faces = new FaceCollectionWrapper(_actualModel.Faces, lod);
        }

        public MPD_CollectionType Collection { get; }

        private ISGL_Model _actualModel;
        public int ModelID { get; }
        public int LevelOfDetail { get; }

        public IReadOnlyList<VECTOR> Vertices => _actualModel.Vertices;

        private class FaceCollectionWrapper : IReadOnlyList<ISGL_ModelFace> {
            public FaceCollectionWrapper(IReadOnlyList<ISGL_ModelFace> faces, int lod) {
                _actualFaces = faces;
                LevelOfDetail = lod;
            }

            private class FaceWrapper : ISGL_ModelFace {
                public FaceWrapper(ISGL_ModelFace face, int lod) {
                    _actualFace = face;
                    _attributes = new AttrWrapper(face.Attributes, lod);
                }

                private ISGL_ModelFace _actualFace;

                public IReadOnlyList<int> VertexIndices => _actualFace.VertexIndices;

                public VECTOR Normal {
                    get => _actualFace.Normal;
                    set => _actualFace.Normal = value;
                }

                private class AttrWrapper : IATTR {
                    public AttrWrapper(IATTR attributes, int lod) {
                        _actualAttributes = attributes;
                        LevelOfDetail     = lod;
                    }

                    private IATTR _actualAttributes;
                    public int LevelOfDetail;

                    public bool Mode_HSSon {
                        get => (LevelOfDetail == 0) ? _actualAttributes.Mode_HSSon : true;
                        set {}
                    }

                    public ushort GouraudShadingTable {
                        get {
                            var gst = _actualAttributes.GouraudShadingTable;
                            if (gst == 0xFFD7)
                                gst = (ushort) (gst + LevelOfDetail);
                            return gst;
                        }
                        set {}
                    }

                    public ushort Mode {
                        get => (ushort) (_actualAttributes.Mode | (LevelOfDetail == 0 ? 0x0000 : 0x1000));
                        set => _actualAttributes.Mode = (ushort) (value | (LevelOfDetail == 0 ? 0x0000 : 0x1000));
                    }

                    public byte       Plane           { get => _actualAttributes.Plane;           set => _actualAttributes.Plane           = value; }

                    public byte       SortAndOptions  { get => _actualAttributes.SortAndOptions;  set => _actualAttributes.SortAndOptions  = value; }
                    public ushort     TextureNo       { get => _actualAttributes.TextureNo;       set => _actualAttributes.TextureNo       = value; }
                    public ushort     ColorNo         { get => _actualAttributes.ColorNo;         set => _actualAttributes.ColorNo         = value; }
                    public ushort     Dir             { get => _actualAttributes.Dir;             set => _actualAttributes.Dir             = value; }

                    public bool       IsTwoSided      { get => _actualAttributes.IsTwoSided;      set => _actualAttributes.IsTwoSided      = value; }

                    public SortOrder  Sort            { get => _actualAttributes.Sort;            set => _actualAttributes.Sort            = value; }
                    public bool       UseTexture      { get => _actualAttributes.UseTexture;      set => _actualAttributes.UseTexture      = value; }
                    public bool       UseLight        { get => _actualAttributes.UseLight;        set => _actualAttributes.UseLight        = value; }

                    public bool       Mode_MSBon      { get => _actualAttributes.Mode_MSBon;      set => _actualAttributes.Mode_MSBon      = value; }
                    public WindowMode Mode_WindowMode { get => _actualAttributes.Mode_WindowMode; set => _actualAttributes.Mode_WindowMode = value; }
                    public bool       Mode_MESHon     { get => _actualAttributes.Mode_MESHon;     set => _actualAttributes.Mode_MESHon     = value; }
                    public bool       Mode_ECdis      { get => _actualAttributes.Mode_ECdis;      set => _actualAttributes.Mode_ECdis      = value; }
                    public bool       Mode_SPdis      { get => _actualAttributes.Mode_SPdis;      set => _actualAttributes.Mode_SPdis      = value; }
                    public ColorMode  Mode_ColorMode  { get => _actualAttributes.Mode_ColorMode;  set => _actualAttributes.Mode_ColorMode  = value; }
                    public DrawMode   Mode_DrawMode   { get => _actualAttributes.Mode_DrawMode;   set => _actualAttributes.Mode_DrawMode   = value; }
                    public bool       CL_Gouraud      { get => _actualAttributes.CL_Gouraud;      set => _actualAttributes.CL_Gouraud      = value; }

                    public string     HtmlColor       { get => _actualAttributes.HtmlColor;       set => _actualAttributes.HtmlColor       = value; }

                    public bool       HFlip           { get => _actualAttributes.HFlip;           set => _actualAttributes.HFlip           = value; }
                    public bool       VFlip           { get => _actualAttributes.VFlip;           set => _actualAttributes.VFlip           = value; }
                }

                private AttrWrapper _attributes;
                public IATTR Attributes {
                    get => _attributes;
                    set {}
                }
            }

            public int Count => _actualFaces.Count;

            public ISGL_ModelFace this[int index] {
                get {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    if (!_faces.ContainsKey(index))
                        _faces.Add(index, new FaceWrapper(_actualFaces[index], LevelOfDetail));
                    return _faces[index];
                }
            }

            public ISGL_ModelFace[] AsArray() => Enumerable.Range(0, Count).Select(x => this[x]).ToArray();

            public IEnumerator<ISGL_ModelFace> GetEnumerator() {
                int length = Count;
                for (int i = 0; i < length; i++)
                    yield return this[i];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private IReadOnlyList<ISGL_ModelFace> _actualFaces;
            public int LevelOfDetail { get; }

            private Dictionary<int, FaceWrapper> _faces = new Dictionary<int, FaceWrapper>();
        }

        public IReadOnlyList<ISGL_ModelFace> Faces { get; }
    }
}
