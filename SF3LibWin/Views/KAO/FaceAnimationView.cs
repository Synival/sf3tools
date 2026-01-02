using CommonLib.Arrays;
using CommonLib.Extensions;
using SF3.Images;
using SF3.Models.Structs.KAO;
using SF3.Types;

namespace SF3.Win.Views.KAO {
    public class FaceAnimationView : AnimatedTextureView {
        private static int c_totalFrames = 240;

        public FaceAnimationView(string name, FaceChunk face, float imageScale = 0) : base(name, imageScale) {
            StartAnimation(face);
        }

        public FaceChunk Face { get; private set; }
        private ITextureData _faceTexture = null;

        public void StartAnimation(FaceChunk face) {
            Face = face;
            if (face == null) {
                ClearAnimation();
                Texture = null;
            }
            else {
                _lastBlinkingFrame = -1;
                _lastTalkingFrame  = -1;
                UpdateTexture(0);
                SetFrame(_faceTexture, 0, 1);
            }
        }

        protected override void OnFrameCompleted() {
            var newFrame = (FrameIndex + 1) % c_totalFrames;
            UpdateTexture(newFrame);
            SetFrame(_faceTexture, newFrame, 1);
        }

        private void UpdateTexture(int frame) {
            // Figure out which animation frames to add.
            var blinkingImage = GetBlinkingImage(frame);
            var talkingImage  = GetTalkingImage(frame);

            var blinkRef = Face.ImageTable[blinkingImage].FrameRef ?? Face.ImageTable[blinkingImage].SubstituteFrameRef ?? 0;
            var talkRef  = Face.ImageTable[talkingImage].FrameRef  ?? Face.ImageTable[talkingImage].SubstituteFrameRef  ?? 0;

            // Don't bother updating the image if the frames are the same.
            if (blinkRef == _lastBlinkingFrame && talkRef == _lastTalkingFrame)
                return;

            // Generate the new image data.
            var newData  = Face.ImageTable[0].ImageData8Bit.Clone() as byte[,];
            if (blinkRef > 0)
                FaceCompositeImage.AddFaceImageToData(newData, Face.ImageTable[blinkRef]);
            if (talkRef > 0)
                FaceCompositeImage.AddFaceImageToData(newData, Face.ImageTable[talkRef]);

            // Generate a texture for it.
            _faceTexture = new TextureData(new ByteArray(newData.To1DArrayTransposed()), 0, Face.Header.Width, Face.Header.Height,
                TexturePixelFormat.Palette1, Face.Palette, false, false, false);
        }

        private int GetBlinkingImage(int frame) {
            frame %= 80;
            if (frame == 39 || frame == 41)
                return 2;
            else if (frame == 40)
                return 3;
            else
                return 1;
        }

        private int GetTalkingImage(int frame) {
            frame %= 60;
            if (frame >= 30)
                return (frame % 6) + 4;
            else
                return 9;
        }

        private int _lastBlinkingFrame = -1;
        private int _lastTalkingFrame = -1;
    }
}
