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
        };
    }
}
