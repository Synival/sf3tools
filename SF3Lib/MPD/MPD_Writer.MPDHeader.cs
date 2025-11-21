using CommonLib.SGL;
using SF3.Models.Structs.MPD;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteMPDHeader(
            MPDHeaderModel header,
            IMPD_Flags flags,
            uint? lightPalettePos,
            uint? lightPositionPos,
            uint? unknown1Pos,
            uint? modelSwitchGroupsPos,
            uint? textureAnimationsPos,
            uint? unknown2Pos,
            uint? groundAnimationPos,
            uint? textureAnimAltPos,
            uint? palette1Pos,
            uint? palette2Pos,
            uint? boundariesPos
        ) {
            var headerAddr = (uint) CurrentOffset;

            // TODO: determine proper map flags
            WriteUShort(header.MapFlags);
            WriteMPDPointer(lightPalettePos);
            WriteMPDPointer(lightPositionPos);
            WriteMPDPointer(unknown1Pos);
            WriteUShort(header.ViewDistance);
            WriteMPDPointer(modelSwitchGroupsPos);
            WriteMPDPointer(textureAnimationsPos);
            WriteMPDPointer(unknown2Pos);
            WriteMPDPointer(groundAnimationPos);
            // TODO: mesh1pos
            WriteMPDPointer(null);
            // TODO: mesh2pos
            WriteMPDPointer(null);
            // TODO: mesh3pos
            WriteMPDPointer(null);
            WriteShort(new CompressedFIXED(header.ModelsPreYRotation / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(header.ModelsViewAngleMin / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(header.ModelsViewAngleMax / 180.0f, 0).RawShort);
            WriteMPDPointer(textureAnimAltPos);
            WriteMPDPointer(palette1Pos ?? headerAddr);
            WriteMPDPointer(palette2Pos ?? headerAddr);
            WriteShort(header.GroundX);
            WriteShort(header.GroundY);
            WriteShort(header.GroundZ);
            WriteShort(new CompressedFIXED(header.GroundAngle, 0).RawShort);
            WriteShort(header.Unknown1);
            WriteShort(header.BackgroundX);
            WriteShort(header.BackgroundY);
            WriteMPDPointer(boundariesPos);
        }
    }
}
