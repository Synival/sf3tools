using SF3.Types;

namespace SF3.MPD {
    public class MPD_ModelInstance : IMPD_ModelInstance {
        public MPD_ModelInstance() {
            OnlyVisibleFromDirection = ModelDirectionType.Unset;
        }

        public MPD_ModelInstance(IMPD_ModelInstance mi) {
            Collection = mi.Collection;
            ID = mi.ID;
            ModelID = mi.ModelID;
            PositionX = mi.PositionX;
            PositionY = mi.PositionY;
            PositionZ = mi.PositionZ;
            AngleX = mi.AngleX;
            AngleY = mi.AngleY;
            AngleZ = mi.AngleZ;
            ScaleX = mi.ScaleX;
            ScaleY = mi.ScaleY;
            ScaleZ = mi.ScaleZ;
            Tag = mi.Tag;
            Flags = mi.Flags;
        }

        public CollectionType Collection { get; set; }
        public int ID { get; set; }
        public int ModelID { get; set; }
        public short PositionX { get; set; }
        public short PositionY { get; set; }
        public short PositionZ { get; set; }
        public float AngleX { get; set; }
        public float AngleY { get; set; }
        public float AngleZ { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }
        public ushort Tag { get; set; }
        public ushort Flags { get; set; }

        public bool AlwaysFacesCamera {
            get => (Flags & 0x08) == 0x08;
            set => Flags = (ushort) ((Flags & ~0x08) | (value ? 0x08 : 0x00));
        }

        public ModelDirectionType OnlyVisibleFromDirection {
            get => ((Flags & 0x10) == 0x10) ? (ModelDirectionType) (Flags & 0x07) : ModelDirectionType.Unset;
            set => Flags = (ushort) ((Flags & 0x07) | (((((short) value) & 0x07) == (short) ModelDirectionType.Unset) ? 0 : (((ushort) value) & 0x07)));
        }
    }
}
