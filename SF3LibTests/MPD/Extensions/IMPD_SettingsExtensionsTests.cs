using SF3.MPD.Extensions;
using SF3.Types;
using static SF3.Tests.Utils.MPD_TestUtils;

namespace SF3.Tests.MPD.Extensions {
    [TestClass]
    public class IMPD_SettingsExtensionsTests {
        [TestMethod]
        public void ToJSON_String_WithScenario1MPD_ReturnsAllVariables() {
            var mpdFile = MakeMPD_File(ScenarioType.Scenario1, "BALSA.MPD");
            var settingsJson = mpdFile.Settings.ToJSON_String();

            const string expectedResult =
                "{\r\n" +
                "  \"HasSurfaceModel\": false,\r\n" +
                "  \"ForceLowMemoryModels\": false,\r\n" +
                "  \"NarrowAngleBasedLightmap\": false,\r\n" +
                "  \"SetMSBForGroundPalette\": false,\r\n" +
                "  \"HasSurfaceTextureRotation\": false,\r\n" +
                "  \"AddDotProductBasedNoiseToStandardLightmap\": false,\r\n" +
                "  \"KeepTexturelessFlatTiles\": false,\r\n" +
                "  \"UnknownHeaderFlag\": false,\r\n" +
                "  \"ModelsYRotation\": -180.0,\r\n" +
                "  \"ModelsViewDistance\": 80,\r\n" +
                "  \"ModelsViewAngleMin\": -107.995605,\r\n" +
                "  \"ModelsViewAngleMax\": 107.995605,\r\n" +
                "  \"UnknownHeaderSetting\": 0,\r\n" +
                "  \"LightPaletteAdjustment\": {\r\n" +
                "    \"R\": 0,\r\n" +
                "    \"G\": 0,\r\n" +
                "    \"B\": 0\r\n" +
                "  },\r\n" +
                "  \"GroundPaletteAdjustment\": {\r\n" +
                "    \"R\": 0,\r\n" +
                "    \"G\": 0,\r\n" +
                "    \"B\": 0\r\n" +
                "  },\r\n" +
                "  \"ShadowTransparency\": 15,\r\n" +
                "  \"AreGroundAnimationsDummiedOut\": false,\r\n" +
                "  \"IsGradientDummiedOut\": false,\r\n" +
                "  \"IsUnknown2TableDummiedOut\": false,\r\n" +
                "  \"AreIgnoredTexturesDummiedOut\": false,\r\n" +
                "  \"IgnoreGroundImage\": false,\r\n" +
                "  \"IgnoreGroundTiledImage\": false,\r\n" +
                "  \"IgnoreSkyImage\": false,\r\n" +
                "  \"IgnoreBackgroundImage\": false,\r\n" +
                "  \"IgnoreForegroundTiledImage\": false,\r\n" +
                "  \"HasBattleBackground\": false,\r\n" +
                "  \"IgnoreSurfaceModel\": false,\r\n" +
                "  \"IsUnknown2InLaterFileAfterGradient\": false\r\n" +
                "}";

            Assert.AreEqual(expectedResult, settingsJson);
        }
    }
}
