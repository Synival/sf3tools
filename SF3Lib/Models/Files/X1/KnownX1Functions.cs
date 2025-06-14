﻿using System.Collections.Generic;

namespace SF3.Models.Files.X1 {
    public static class KnownX1Functions {
        public readonly struct FuncInfo {
            public FuncInfo(string typeName, string name) {
                TypeName = typeName;
                Name = name;
            }

            public readonly string TypeName;
            public readonly string Name;
        }

        public static Dictionary<FuncInfo, ushort[]> AllKnownFunctions = new Dictionary<FuncInfo, ushort[]>() {
            { new FuncInfo("InstantiateModelsFunc", "instantiateModelsScn1(ModelInstance *modelTable)"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD213, 0xE100, 0x2210, 0xD112, 0x6112,
                0x6010, 0xC980, 0x2008, 0x8903, 0xD210, 0xD111, 0xA003, 0x2212,
                0xD20E, 0xD110, 0x2212, 0xD110, 0x2142, 0x6142, 0x2118, 0x8D0D,
                0x6843, 0xD90E, 0x5783, 0x5682, 0x5581, 0x5184, 0x6482, 0x490B,
                0x2F16, 0x7814, 0x6182, 0x2118, 0x8FF4, 0x7F04, 0x4F26, 0x69F6,
                0x000B, 0x68F6
            }},

            { new FuncInfo("InstantiateModelsFunc", "instantiateModelsScn2(ModelInstance *modelTable)"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD218, 0xE100, 0x2210, 0xD117, 0x6112,
                0x6010, 0xE102, 0x2109, 0x2118, 0x8D07, 0xC980, 0x2008, 0x8B04,
                0xD213, 0xD114, 0x2212, 0xA004, 0x71FC, 0xD211, 0xD112, 0x2212,
                0xD112, 0x6111, 0xD212, 0x2211, 0xD112, 0x2142, 0x6142, 0x2118,
                0x8D0D, 0x6843, 0xD910, 0x5783, 0x5682, 0x5581, 0x5184, 0x6482,
                0x490B, 0x2F16, 0x7814, 0x6182, 0x2118, 0x8FF4, 0x7F04, 0x4F26,
                0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("InstantiateModelsFunc", "instantiateModelsX1BTL128(ModelInstance *modelTable)"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD112, 0x6112, 0x6010, 0xC980, 0x2008,
                0x8903, 0xD210, 0xD110, 0xA003, 0x2212, 0xD20E, 0xD10F, 0x2212,
                0xD10F, 0x2142, 0x6142, 0x2118, 0x8D0D, 0x6843, 0xD90D, 0x5783,
                0x5682, 0x5581, 0x5184, 0x6482, 0x490B, 0x2F16, 0x7814, 0x6182,
                0x2118, 0x8FF4, 0x7F04, 0x4F26, 0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("InstantiateModelsFunc", "instantiateModelsX1BTL206(ModelInstance *modelTable)"), new ushort[] {
                0x2F86, 0x2F96, 0x2FA6, 0x2FB6, 0x2FC6, 0x4F22, 0xD225, 0xE100,
                0x2210, 0xD125, 0x6112, 0x6010, 0xE102, 0x2109, 0x2118, 0x8D08,
                0x6C43, 0xC980, 0x2008, 0x8B04, 0xD220, 0xD121, 0x2212, 0xA004,
                0x71FC, 0xD21E, 0xD11F, 0x2212, 0xD11F, 0x6111, 0xD21F, 0x2211,
                0xD119, 0x6112, 0x6010, 0xC940, 0x2008, 0x8911, 0xD01C, 0x400B,
                0xE800, 0xD11A, 0x6911, 0x3893, 0x8D0A, 0x6A0F, 0xDB19, 0x658F,
                0x67AD, 0xE603, 0x4B0B, 0xE400, 0x7801, 0x3893, 0x8FF8, 0x658F,
                0xD115, 0x21C2, 0x61C2, 0x2118, 0x8D0D, 0x68C3, 0xD913, 0x5783,
                0x5682, 0x5581, 0x5184, 0x6482, 0x490B, 0x2F16, 0x7814, 0x6182,
                0x2118, 0x8FF4, 0x7F04, 0x4F26, 0x6CF6, 0x6BF6, 0x6AF6, 0x69F6,
                0x000B, 0x68F6
            }},

            { new FuncInfo("InstantiateModelsFunc", "instantiateModelsX1BTL228(ModelInstance *modelTable)"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD219, 0xE100, 0x2210, 0xD118, 0x6112,
                0x6010, 0xE102, 0x2109, 0x2118, 0x8D07, 0xC980, 0x2008, 0x8B04,
                0xD214, 0xD115, 0x2212, 0xA004, 0x71FC, 0xD212, 0xD113, 0x2212,
                0xD113, 0x6111, 0xD213, 0x2211, 0xD113, 0x2142, 0xD213, 0xE100,
                0x2212, 0x6142, 0x2118, 0x8D0D, 0x6843, 0xD911, 0x5783, 0x5682,
                0x5581, 0x5184, 0x6482, 0x490B, 0x2F16, 0x7814, 0x6182, 0x2118,
                0x8FF4, 0x7F04, 0x4F26, 0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXScrollZAndRipple()"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD215, 0x6121, 0x7101, 0x2211, 0xD114,
                0x6112, 0xD914, 0x6692, 0x7146, 0x6511, 0xD813, 0x6482, 0xD113,
                0x410B, 0x4528, 0x9117, 0x6292, 0xD711, 0x321C, 0x2922, 0x9113,
                0x6382, 0x3277, 0x331C, 0x8F02, 0x2832, 0xE100, 0x2912, 0x3377,
                0x8F01, 0xE100, 0x2812, 0xD40B, 0xD10B, 0x410B, 0x0009, 0x4F26,
                0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXScrollZAndOscillateAndRippleTogether()"), new ushort[] {
                0x2F86, 0x2F96, 0x2FA6, 0x4F22, 0xD629, 0x6761, 0xD129, 0x637F,
                0x331D, 0x6233, 0x010A, 0x313C, 0x4121, 0x4121, 0x4121, 0x4121,
                0x4121, 0x4200, 0x322A, 0x3128, 0xE22E, 0x0127, 0xD022, 0x7701,
                0x011A, 0x3318, 0x633F, 0x9138, 0x231F, 0x2671, 0x041A, 0x400B,
                0x644F, 0xDA1E, 0x6503, 0x35AC, 0xD01D, 0x400B, 0x64A3, 0xD11D,
                0x2102, 0xD91D, 0x6692, 0xD11D, 0x6112, 0xD81D, 0x6482, 0x7146,
                0x6511, 0x7506, 0x4528, 0x6103, 0x4104, 0x0129, 0x310C, 0x4121,
                0x3518, 0xD118, 0x410B, 0x360C, 0x9118, 0x6292, 0xD716, 0x321C,
                0x2922, 0x9114, 0x6382, 0x3277, 0x331C, 0x8F02, 0x2832, 0xE100,
                0x2912, 0x3377, 0x8F01, 0xE100, 0x2812, 0xD110, 0x410B, 0x64A3,
                0x4F26, 0x6AF6, 0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollZAndRipple()"), new ushort[] {
                0x2F86, 0x4F22, 0xD10E, 0x6112, 0xD80E, 0x6682, 0x6213, 0x7246,
                0x6521, 0x7144, 0x6411, 0x4528, 0xD10B, 0x410B, 0x4428, 0x6182,
                0x920D, 0x312C, 0xD209, 0x3127, 0x8F02, 0x2812, 0xE100, 0x2812,
                0xD407, 0xD108, 0x410B, 0x0009, 0x4F26, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXZAndRipple()"), new ushort[] {
                0x2F86, 0x4F22, 0xD10D, 0x6112, 0xD80D, 0x6482, 0x7146, 0x6511,
                0x6643, 0xD10C, 0x410B, 0x4528, 0x6182, 0x920D, 0x312C, 0xD20A,
                0x3127, 0x8F02, 0x2812, 0xE100, 0x2812, 0xD408, 0xD108, 0x410B,
                0x0009, 0x4F26, 0x000B, 0x68F6
            }},

            // (This looks way too generic to be useful but there seem to be no false positives!)
            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_Ripple()"), new ushort[] {
                0x4F22, 0xD403, 0xD103, 0x410B, 0x0009, 0x4F26, 0x000B, 0x0009
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXAndRipple_UnusedScrollZ()"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD214, 0x6121, 0x7101, 0x2211, 0xD113,
                0x6112, 0xD913, 0x6692, 0x7146, 0x6511, 0xD812, 0x6482, 0xD112,
                0x410B, 0x4528, 0x9115, 0x6392, 0xD710, 0x2932, 0x6282, 0x3377,
                0x321C, 0x8F02, 0x2822, 0xE100, 0x2912, 0x3277, 0x8F01, 0xE100,
                0x2812, 0x9407, 0xD10A, 0x410B, 0x0009, 0x4F26, 0x69F6, 0x000B,
                0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollZ()"), new ushort[] {
                0x2F86, 0x4F22, 0xD10C, 0x6112, 0xD80C, 0x6682, 0x6213, 0x7246,
                0x6521, 0x7144, 0x6411, 0x4528, 0xD109, 0x410B, 0x4428, 0x6182,
                0x9209, 0x312C, 0xD207, 0x3127, 0x8F02, 0x2812, 0xE100, 0x2812,
                0x4F26, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ControllableScrollXAndHeightAndRipple()"), new ushort[] {
                0x2F86, 0x4F22, 0x9426, 0xD014, 0x400B, 0x0009, 0x2008, 0x8B1E,
                0xD112, 0x6112, 0xD812, 0x6482, 0x6213, 0x7146, 0x6511, 0x7248,
                0x6621, 0xD110, 0x6112, 0x4628, 0x3518, 0xD10F, 0x410B, 0x4528,
                0x6282, 0xD10E, 0x6112, 0x3218, 0xD10D, 0x3217, 0x8F03, 0x2822,
                0x9109, 0x312C, 0x2812, 0xD40B, 0xD10B, 0x410B, 0x0009, 0x4F26,
                0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_IncrementCounterAndScrollXScrollZ()"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD213, 0x6121, 0x7101, 0x2211, 0xD112,
                0x6112, 0xD912, 0x6692, 0x7146, 0x6511, 0xD811, 0x6482, 0xD111,
                0x410B, 0x4528, 0x9113, 0x6292, 0xD70F, 0x321C, 0x2922, 0x910F,
                0x6382, 0x3277, 0x331C, 0x8F02, 0x2832, 0xE100, 0x2912, 0x3377,
                0x8F01, 0xE100, 0x2812, 0x4F26, 0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXScrollZAndOscillateAndRippleTogetherScn3()"), new ushort[] {
                0x2F86, 0x2F96, 0x2FA6, 0x4F22, 0xD629, 0x6761, 0xD129, 0x637F,
                0x331D, 0x6233, 0x010A, 0x313C, 0x4121, 0x4121, 0x4121, 0x4121,
                0x4121, 0x4200, 0x322A, 0x3128, 0xE22E, 0x0127, 0xD022, 0x7701,
                0x011A, 0x3318, 0x633F, 0x9138, 0x231F, 0x2671, 0x041A, 0x400B,
                0x644F, 0xDA1E, 0x6503, 0x35AC, 0xD01D, 0x400B, 0x64A3, 0xD11D,
                0x2102, 0xD91D, 0x6692, 0xD11D, 0x6112, 0xD81D, 0x6482, 0x714E,
                0x6511, 0x7506, 0x4528, 0x6103, 0x4104, 0x0129, 0x310C, 0x4121,
                0x3518, 0xD118, 0x410B, 0x360C, 0x9118, 0x6292, 0xD716, 0x321C,
                0x2922, 0x9114, 0x6382, 0x3277, 0x331C, 0x8F02, 0x2832, 0xE100,
                0x2912, 0x3377, 0x8F01, 0xE100, 0x2812, 0xD110, 0x410B, 0x64A3,
                0x4F26, 0x6AF6, 0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXScrollZAndOscillateAndRippleSeparately()"), new ushort[] {
                0x2F86, 0x2F96, 0x2FA6, 0x4F22, 0xD728, 0x6371, 0xD128, 0x623F,
                0x321D, 0x040A, 0xD127, 0x410B, 0x342C, 0x6123, 0x4100, 0x311A,
                0x3418, 0xE15A, 0x0417, 0xD024, 0x7301, 0x011A, 0x3218, 0x622F,
                0x9138, 0x221F, 0x2731, 0x041A, 0x400B, 0x644F, 0xDA1F, 0x6503,
                0xD41F, 0xD020, 0x400B, 0x35AC, 0xD11F, 0x2102, 0xD91F, 0x6692,
                0xD11F, 0x6112, 0xD81F, 0x6482, 0x714E, 0x6511, 0x7506, 0x4528,
                0x6103, 0x4104, 0x0129, 0x310C, 0x4121, 0x3518, 0xD11A, 0x410B,
                0x360C, 0x9118, 0x6292, 0xD719, 0x321C, 0x2922, 0x9114, 0x6382,
                0x3277, 0x331C, 0x8F02, 0x2832, 0xE100, 0x2912, 0x3377, 0x8F01,
                0xE100, 0x2812, 0xD112, 0x410B, 0x64A3, 0x4F26, 0x6AF6, 0x69F6,
                0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXScrollZ_BrokenOscillateAndRipple()"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD726, 0x6371, 0xD126, 0x623F, 0x321D,
                0x040A, 0xD125, 0x410B, 0x342C, 0x6123, 0x4100, 0x311A, 0x3418,
                0xE15A, 0x0417, 0xD021, 0x7301, 0x011A, 0x3218, 0x622F, 0x9133,
                0x221F, 0x2731, 0x041A, 0x400B, 0x644F, 0xD11D, 0x6503, 0xD41D,
                0xD01D, 0x400B, 0x351C, 0xD11D, 0x2102, 0xD11D, 0x6112, 0xD91D,
                0x6692, 0x714E, 0x6511, 0xD81C, 0x7506, 0x4528, 0x6103, 0x4104,
                0x0129, 0x310C, 0x4121, 0x3518, 0xD118, 0x410B, 0x6482, 0x9114,
                0x6292, 0xD717, 0x321C, 0x2922, 0x9110, 0x6382, 0x3277, 0x331C,
                0x8F02, 0x2832, 0xE100, 0x2912, 0x3377, 0x8F01, 0xE100, 0x2812,
                0x4F26, 0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXScrollZ_ExtremeOscillateAndRipple()"), new ushort[] {
                0x2F86, 0x2F96, 0x2FA6, 0x4F22, 0xD629, 0x6761, 0xD129, 0x637F,
                0x331D, 0x6233, 0x010A, 0x313C, 0x4121, 0x4121, 0x4121, 0x4121,
                0x4121, 0x4200, 0x322A, 0x3128, 0xE22E, 0x0127, 0xD022, 0x7701,
                0x011A, 0x3318, 0x633F, 0x9137, 0x231F, 0x2671, 0x041A, 0x400B,
                0x644F, 0xDA1E, 0x6503, 0x35AC, 0xD01D, 0x400B, 0x64A3, 0xD11D,
                0x2102, 0xD11D, 0x6112, 0xD91D, 0x6692, 0x714E, 0x6511, 0xD81C,
                0x7506, 0x4528, 0x6103, 0x4104, 0x0129, 0x310C, 0x4121, 0x3518,
                0xD118, 0x410B, 0x6482, 0x9118, 0x6292, 0xD717, 0x321C, 0x2922,
                0x9114, 0x6382, 0x3277, 0x331C, 0x8F02, 0x2832, 0xE100, 0x2912,
                0x3377, 0x8F01, 0xE100, 0x2812, 0xD110, 0x410B, 0x64A3, 0x4F26,
                0x6AF6, 0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ScrollXScrollZ()"), new ushort[] {
                0x2F86, 0x2F96, 0x4F22, 0xD111, 0x6112, 0xD911, 0x6692, 0x7146,
                0x6511, 0xD810, 0x6482, 0xD110, 0x410B, 0x4528, 0x9113, 0x6292,
                0xD70E, 0x321C, 0x2922, 0x910F, 0x6382, 0x3277, 0x331C, 0x8F02,
                0x2832, 0xE100, 0x2912, 0x3377, 0x8F01, 0xE100, 0x2812, 0x4F26,
                0x69F6, 0x000B, 0x68F6
            }},

            { new FuncInfo("GroundPlaneTickFunc", "updateWaterFunc_ControllableScrollZ_UnusedScrollX()"), new ushort[] {
                0x2F86, 0x4F22, 0xD217, 0x6121, 0x7101, 0x2211, 0xD116, 0x6112,
                0xD816, 0x6682, 0x6213, 0x7246, 0x6521, 0x7148, 0x6411, 0x4528,
                0xD113, 0x410B, 0x4428, 0xD713, 0x6272, 0x911A, 0x6323, 0x331C,
                0x2732, 0x6282, 0xD110, 0x6112, 0x321C, 0xD110, 0x3217, 0x8F03,
                0x2822, 0xD10F, 0x312C, 0x2812, 0xD20E, 0x3327, 0x8F01, 0xE100,
                0x2712, 0x6182, 0x3127, 0x8F01, 0xE100, 0x2812, 0x4F26, 0x000B,
                0x68F6
            }},

            { new FuncInfo("Function", "updateWaterRipple(int amount)"), new ushort[] {
                0x2F86, 0xD012, 0xD612, 0x6362, 0xD112, 0x6233, 0x2219, 0x6729,
                0x677F, 0x6133, 0x314C, 0x2612, 0xE500, 0xD80F, 0x9414, 0xD60F,
                0x6173, 0x4108, 0x6313, 0x338C, 0x6162, 0x7701, 0x6236, 0x3747,
                0x312C, 0x2012, 0x8F02, 0x7004, 0x6383, 0xE700, 0x7501, 0x3547,
                0x8FF2, 0x7604, 0x000B, 0x68F6
            }},

            { new FuncInfo("UnknownThinkFunc", "applyWaterRippleScn1()"), new ushort[] {
                0x4F22, 0xD105, 0x9606, 0xD505, 0xD405, 0x410B, 0x0009, 0x4F26,
                0x000B, 0x0009, 0x0800
            }},

            { new FuncInfo("UnknownThinkFunc", "applyWaterRipple()"), new ushort[] {
                0x4F22, 0xD107, 0x6110, 0x2118, 0x8B05, 0xD106, 0x9606, 0xD506,
                0xD406, 0x410B, 0x0009, 0x4F26, 0x000B, 0x0009, 0x0800
            }},

            { new FuncInfo("UnknownThinkFunc", "applySomethingSimilarWaterRipple()"), new ushort[] {
                0x4F22, 0xD107, 0x6110, 0x2118, 0x8B05, 0xD106, 0x9606, 0xD506,
                0xD406, 0x410B, 0x0009, 0x4F26, 0x000B, 0x0009, 0x0200
            }},

            // Param1: Usually 0x66
            // Param2: Usually 0x00200000
            { new FuncInfo("Function", "initRippleMap(int amplitude, int frequency)"), new ushort[] {
                0x2F86, 0x2F96, 0x2FA6, 0x2FB6, 0x2FC6, 0x2FD6, 0x2FE6, 0x6C43,
                0xDA12, 0x6453, 0x4411, 0x8D02, 0x4F22, 0x911E, 0x341C, 0xD110,
                0x410B, 0xE800, 0x6B4F, 0xE900, 0xDE0E, 0xDD0F, 0x6183, 0x31BC,
                0x681F, 0x4E0B, 0x6483, 0x6503, 0x4D0B, 0x64C3, 0x2A02, 0x7901,
                0x910B, 0x3917, 0x8FF2, 0x7A04, 0x4F26, 0x6EF6, 0x6DF6, 0x6CF6,
                0x6BF6, 0x6AF6, 0x69F6, 0x000B, 0x68F6
            }},

            // Blacksmith data pointer is +0x48 from start
            { new FuncInfo("BlacksmithFunctionBranch", "blacksmithRelatedBranchScn1"), new ushort[] {
                0xD211, 0x6121, 0x2118, 0x8D0C, 0xE300, 0xD710, 0x6121, 0x3140
            }},

            // Blacksmith data pointer is +0x4C from start
            { new FuncInfo("BlacksmithFunctionBranch", "blacksmithRelatedBranchScn2+"), new ushort[] {
                0xD112, 0x6011, 0x88FF, 0x8D0D, 0xE300, 0xD711, 0x6213, 0x6121
            }},

            // Blacksmith Scn2 func
            { new FuncInfo("BlacksmithFunction", "blacksmithRelatedFunctionScn2+"), new ushort[] {
                0x2F86, 0x2F96, 0x2FA6, 0x4F22, 0x6A43, 0xD810, 0xE60A, 0x480B,
                0xE514, 0xD90F, 0xE60B, 0x6491, 0x480B, 0xE514, 0x6091, 0x88FF,
                0x8D0D, 0xE300, 0xD70B, 0x6293, 0x6121, 0x31A0, 0x8F03, 0x7228,
                0x2732, 0xA005, 0xE002, 0x6021, 0x88FF, 0x8FF5, 0x7301, 0xE000,
                0x4F26, 0x6AF6, 0x69F6, 0x000B, 0x68F6
            }},

/*
                2F86 2F96 2FA6 4F22 6A43 D810 E60A 480B E514 D90F E60B 6491 480B E514 6091 88FF 8D0D E300 D70B 6293 6121 31A0 8F03 7228 2732 A005 E002 6021 88FF 8FF5 7301 E000 4F26 6AF6 69F6 000B 68F6
*/
        };
    }
}
