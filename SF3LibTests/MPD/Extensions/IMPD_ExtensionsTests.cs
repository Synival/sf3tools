using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.Types;
using static SF3.Tests.Utils.MPD_TestUtils;

namespace SF3.Tests.MPD.Extensions {
    [TestClass]
    public class IMPD_ExtensionsTests {
        [TestMethod]
        public void ToJSON_String_WithScenario1MPD_ProducesSomething() {
            var mpdFile = MakeMPD_File(ScenarioType.Scenario1, "BALSA.MPD");

            var mpdJsonStr = mpdFile.ToJSON_String();
            Assert.IsNotNull(mpdJsonStr);

            var mpdJObj = JObject.Parse(mpdJsonStr);
            Assert.IsNotNull(mpdJObj);
        }
    }
}
