using SF3.CHR;
using SF3.Types;

namespace SF3.Tests.CHR {
    [TestClass]
    public class CHR_DefTests {
        [TestMethod]
        public void Deserialize_WithEmptyCHR_HasExpectedData() {
            var chrDef = CHR_Def.FromJSON(TestCHRs.EmptyCHR);

            Assert.IsNull(chrDef.JunkAfterFrameTables);
            Assert.AreEqual(0, chrDef.Sprites.Length);
        }

        [TestMethod]
        public void Deserialize_WithTwoEmptySpriteCHR_HasExpectedData() {
            var chrDef = CHR_Def.FromJSON(TestCHRs.TwoEmptySpriteCHR);

            Assert.IsNull(chrDef.JunkAfterFrameTables);
            Assert.AreEqual(2, chrDef.Sprites.Length);

            var sprite0 = chrDef.Sprites[0];
            Assert.AreEqual(sprite0.SpriteName, "Synbios (U)");
            Assert.AreEqual(sprite0.SpriteID, 0);
            Assert.AreEqual(sprite0.Width, 40);
            Assert.AreEqual(sprite0.Height, 40);
            Assert.AreEqual(sprite0.Directions, SpriteDirectionCountType.Four);
            Assert.AreEqual(sprite0.VerticalOffset, 0);
            Assert.AreEqual(sprite0.Unknown0x08, 20);
            Assert.AreEqual(sprite0.CollisionSize, 40);
            Assert.AreEqual(sprite0.Scale, 1.00f);
            Assert.AreEqual(sprite0.PromotionLevel, 0);
            Assert.AreEqual(sprite0.FrameGroupsForSpritesheets, null);
            Assert.AreEqual(sprite0.AnimationsForSpritesheetAndDirections, null);

            var sprite1 = chrDef.Sprites[1];
            Assert.AreEqual(sprite1.SpriteName, "Synbios (P1)");
            Assert.AreEqual(sprite1.SpriteID, 0);
            Assert.AreEqual(sprite1.Width, 40);
            Assert.AreEqual(sprite1.Height, 40);
            Assert.AreEqual(sprite1.Directions, SpriteDirectionCountType.Four);
            Assert.AreEqual(sprite1.VerticalOffset, 0);
            Assert.AreEqual(sprite1.Unknown0x08, 20);
            Assert.AreEqual(sprite1.CollisionSize, 40);
            Assert.AreEqual(sprite1.Scale, 1.00f);
            Assert.AreEqual(sprite1.PromotionLevel, 1);
            Assert.AreEqual(sprite1.FrameGroupsForSpritesheets, null);
            Assert.AreEqual(sprite1.AnimationsForSpritesheetAndDirections, null);
        }
    }
}
