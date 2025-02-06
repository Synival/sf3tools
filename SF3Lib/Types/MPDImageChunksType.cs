using System;

namespace SF3.Types {
    public enum MPDImageChunksType {
        Unknown                  = 0,
        Nothing                  = 1,
        RepeatingGround          = 2,
        SkyBox                   = 3,
        RepeatingGroundAndSkybox = 4,
        TiledGround              = 5,
        TiledGroundAndSkybox     = 6,
        BackgroundWithTiledForeground = 7,
    }

    public static class MPDImageChunksTypeExtensions {
        public static MPDImageChunksType FromMapFlags(ushort mapFlags, ScenarioType scenario) {
            var mf = mapFlags & ApplicableMapFlags(scenario);

            switch (scenario) {
                case ScenarioType.Scenario1:
                    switch (mf) {
                        case 0x0000:
                            return MPDImageChunksType.Nothing;
                        case 0x0050:
                            return MPDImageChunksType.BackgroundWithTiledForeground;
                        case 0x0400:
                            return MPDImageChunksType.RepeatingGround;
                        case 0x1000:
                            return MPDImageChunksType.TiledGround;
                        case 0x2000:
                            return MPDImageChunksType.SkyBox;
                        case 0x2400:
                            return MPDImageChunksType.RepeatingGroundAndSkybox;
                        case 0x3000:
                            return MPDImageChunksType.TiledGroundAndSkybox;

                        case 0x1400:
                        case 0x3400:
                            try {
                                throw new ArgumentException($"{mapFlags}: Indicates both RepeatingGround (0x0400) and TileGround (0x1000)");
                            }
                            catch { }
                            return MPDImageChunksType.Unknown;
                    }
                    break;

                case ScenarioType.Scenario2:
                case ScenarioType.Scenario3:
                case ScenarioType.PremiumDisk:
                    switch (mf) {
                        case 0x0000:
                            return MPDImageChunksType.Nothing;
                        case 0x0050:
                            return MPDImageChunksType.BackgroundWithTiledForeground;
                        case 0x0400:
                            return MPDImageChunksType.RepeatingGround;
                        case 0x1000:
                            return MPDImageChunksType.TiledGround;
                    }
                    break;

                default:
                    return MPDImageChunksType.Unknown;
            }

            try {
                throw new InvalidOperationException($"Unhandled MPDImageChunksType for {scenario} map flags: {mf}");
            }
            catch { }
            return MPDImageChunksType.Unknown;
        }

        public static ushort ApplicableMapFlags(ScenarioType scenario) {
            switch (scenario) {
                case ScenarioType.Scenario1:
                    return 0x3450;

                case ScenarioType.Scenario2:
                case ScenarioType.Scenario3:
                case ScenarioType.PremiumDisk:
                    return 0x1450;

                default:
                    return 0x0000;
            }
        }

        public static ushort ToMapFlags(this MPDImageChunksType type, ScenarioType scenario) {
            switch (scenario) {
                case ScenarioType.Scenario1:
                    switch (type) {
                        case MPDImageChunksType.Unknown:
                        case MPDImageChunksType.Nothing:
                            return 0x0000;
                        case MPDImageChunksType.BackgroundWithTiledForeground:
                            return 0x0050;
                        case MPDImageChunksType.RepeatingGround:
                            return 0x0400;
                        case MPDImageChunksType.TiledGround:
                            return 0x1000;
                        case MPDImageChunksType.SkyBox:
                            return 0x2000;
                        case MPDImageChunksType.RepeatingGroundAndSkybox:
                            return 0x2400;
                        case MPDImageChunksType.TiledGroundAndSkybox:
                            return 0x3000;
                    }
                    break;

                case ScenarioType.Scenario2:
                case ScenarioType.Scenario3:
                case ScenarioType.PremiumDisk:
                    switch (type) {
                        case MPDImageChunksType.Unknown:
                        case MPDImageChunksType.Nothing:
                            return 0x0000;
                        case MPDImageChunksType.BackgroundWithTiledForeground:
                            return 0x0050;
                        case MPDImageChunksType.RepeatingGround:
                            return 0x0400;
                        case MPDImageChunksType.TiledGround:
                            return 0x1000;
                    }
                    break;

                default:
                    return 0x0000;
            }

            try {
                throw new InvalidOperationException($"Unhandled map flags for {scenario} MPDImageChunksType: {type}");
            }
            catch { }
            return 0x0000;
        }

        public static bool HasRepeatingGround(this MPDImageChunksType type)
            => (type == MPDImageChunksType.RepeatingGround || type == MPDImageChunksType.RepeatingGroundAndSkybox);

        public static bool HasTiledGround(this MPDImageChunksType type)
            => (type == MPDImageChunksType.TiledGround);

        public static bool HasSkyBox(this MPDImageChunksType type)
            => (type == MPDImageChunksType.SkyBox || type == MPDImageChunksType.RepeatingGroundAndSkybox);

        public static bool HasBackground(this MPDImageChunksType type)
            => (type == MPDImageChunksType.BackgroundWithTiledForeground);
    }
}
