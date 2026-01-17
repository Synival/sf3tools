using System;
using SF3.MPD;
using SF3.Types;

namespace SF3.Extensions {
    public static class IMPD_Extensions {
        public static ushort GetHeaderFlags(this IMPD mpd, ScenarioType scenario) {
            switch (scenario) {
                case ScenarioType.Ship2:
                case ScenarioType.Other:
                case ScenarioType.Scenario1:
                    return GetScenario1HeaderFlags(mpd);

                case ScenarioType.Scenario2:
                    return GetScenario2HeaderFlags(mpd);

                case ScenarioType.Scenario3:
                    return GetScenario3HeaderFlags(mpd);

                case ScenarioType.PremiumDisk:
                    return GetPremiumDiskHeaderFlags(mpd);

                default:
                    throw new ArgumentException($"Unhandled scenario '{scenario}'");
            }
        }

        // Macro-like function for easier code readability
        private static ushort F(ushort flag, bool condition) => condition ? flag : (ushort) 0x0000;

        public static ushort GetScenario1HeaderFlags(this IMPD mpd) {
            return (ushort) (
                F(0x0001, true) // always on
              | F(0x0002, true) // always on
              | F(0x0004, mpd.Flags.Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap)
              | F(0x0008, mpd.Flags.Bit_0x0008_KeepTexturelessFlatTiles)
              | F(0x0010, mpd.Flags.Bit_0x0010_HasTileBasedForegroundImage)
              | F(0x0020, mpd.Flags.Bit_0x0020_Unknown)
              | F(0x0040, mpd.Flags.Bit_0x0040_HasBackgroundImage)
              | F(0x0080, mpd.Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures || mpd.Flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures)
              | F(0x0100, mpd.Flags.Bit_0x0100_HasModels)
              | F(0x0200, mpd.Flags.Bit_0x0200_HasSurfaceModel)
              | F(0x0400, mpd.Flags.Bit_0x0400_HasGroundImage)
              | F(0x0800, mpd.Flags.Bit_0x0800_Unused)
              | F(0x1000, mpd.Flags.Bit_0x1000_HasTileBasedGroundImage)
              | F(0x2000, mpd.Flags.Bit_0x2000_HasBattleSky ||
                    (mpd.Flags.Bit_0x0800_HasCutsceneSky && mpd.Planes.SkyImage != null))
              | F(0x4000, mpd.Flags.Bit_0x4000_Unused)
              | F(0x8000, mpd.Flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
            );
        }

        public static ushort GetScenario2HeaderFlags(this IMPD mpd) {
            return (ushort) (
                F(0x0001, true) // always on
              | F(0x0002, true) // always on
              | F(0x0004, mpd.Flags.Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap)
              | F(0x0008, mpd.Flags.Bit_0x0008_KeepTexturelessFlatTiles)
              | F(0x0010, mpd.Flags.Bit_0x0010_HasTileBasedForegroundImage)
              | F(0x0020, mpd.Flags.Bit_0x0020_Unknown)
              | F(0x0040, mpd.Flags.Bit_0x0040_HasBackgroundImage)
              | F(0x0080, mpd.Flags.Bit_0x0080_SetMSBForGroundPalette)
              | F(0x0100, mpd.Flags.Bit_0x0100_HasModels)
              | F(0x0200, mpd.Flags.Bit_0x0200_HasSurfaceModel)
              | F(0x0400, mpd.Flags.Bit_0x0400_HasGroundImage)
              | F(0x0800, mpd.Flags.Bit_0x0800_HasCutsceneSky | mpd.Flags.Bit_0x2000_HasBattleSky)
              | F(0x1000, mpd.Flags.Bit_0x1000_HasTileBasedGroundImage)
              | F(0x2000, mpd.Flags.Bit_0x2000_NarrowAngleBasedLightmap)
              | F(0x4000, mpd.Flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures || mpd.Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures)
              | F(0x8000, mpd.Flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
            );
        }

        // TODO: Implement properly!!
        public static ushort GetScenario3HeaderFlags(this IMPD mpd) => GetScenario1HeaderFlags(mpd);

        // Flags are the same.
        public static ushort GetPremiumDiskHeaderFlags(this IMPD mpd) => GetScenario3HeaderFlags(mpd);
    }
}
