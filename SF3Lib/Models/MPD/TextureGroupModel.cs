using CommonLib.Attributes;
using SF3.Attributes;
using SF3.RawEditors;

namespace SF3.Models.MPD {
    public class TextureGroupModel : Model {
        private readonly int _textureIdAddress;
        private readonly int _widthAddress;
        private readonly int _heightAddress;
        private readonly int _unknownAddress;
        private readonly int _framesAddress;

        public TextureGroupModel(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x0A) {
            _textureIdAddress = Address + 0x00; // 2 bytes
            _widthAddress     = Address + 0x02; // 2 bytes
            _heightAddress    = Address + 0x04; // 2 bytes
            _unknownAddress   = Address + 0x06; // 2 bytes
            _framesAddress    = Address + 0x08; // variable; multiple of 4 bytes

            // Determine the number of frames. That will determine the size of this texture group.
            // TODO: how in the world do we model this?? a separate table each?
            int frameCount = 0;
            int pos = _framesAddress;
            if (TextureID != 0xFFFF) {
                while (true) {
                    var frameOffset = (ushort) Editor.GetWord(pos);
                    pos += 2;
                    if (frameOffset == 0xFFFE)
                        break;
                    pos += 2;
                    frameCount++;
                }
            }

            Size = pos - Address;
            NumFrames = frameCount;
        }

        [BulkCopy]
        [ViewModelData(displayName: "Texture ID", displayOrder: 0, displayFormat: "X2")]
        public int TextureID {
            get => Editor.GetWord(_textureIdAddress);
            set => Editor.SetWord(_textureIdAddress, value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "Width", displayOrder: 1)]
        public int Width {
            get => Editor.GetWord(_widthAddress);
            set => Editor.SetWord(_widthAddress, value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "Height", displayOrder: 2)]
        public int Height {
            get => Editor.GetWord(_heightAddress);
            set => Editor.SetWord(_heightAddress, value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "Unknown", displayOrder: 3, displayFormat: "X4")]
        public int Unknown {
            get => Editor.GetWord(_unknownAddress);
            set => Editor.SetWord(_unknownAddress, value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "# Frames", displayOrder: 4, isReadOnly: true)]
        public int NumFrames { get; }
    }
}
