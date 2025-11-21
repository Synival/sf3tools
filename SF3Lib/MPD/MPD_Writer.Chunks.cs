using System.Linq;
using CommonLib.Logging;
using CommonLib.Types;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteChunks(IMPD_File mpd) {
            // Chunk[0] is always empty.
            WriteEmptyChunk();

            // TODO: check for this, and get memory mapping stuff!!
            // Chunk[1] is always models if it exists.
            var mc = mpd.ModelCollections.FirstOrDefault(x => x?.CollectionType == ModelCollectionType.PrimaryModels);
            if (mc == null)
                WriteEmptyChunk();
            else
                WriteModelChunk(mc.GetSGLModels(), mc.GetModelInstances());

            // TODO: actual chunks!!
            int chunkTableSize = 20;
            for (int i = 2; i < chunkTableSize; i++)
                WriteEmptyChunk();
        }

        public void WriteEmptyChunk() {
            StartNewChunk();
            FinishCurrentChunk();
        }

        private void StartNewChunk() {
            if (_currentChunkStart != null)
                Logger.WriteLine($"Chunk[{_currentChunks}] started again before it was finished", LogType.Error);
            _currentChunkStart = CurrentOffset;
            AtOffset(0x2000 + _currentChunks * 0x08, curOffset => WriteMPDPointer((uint) curOffset));
        }

        private void FinishCurrentChunk() {
            if (_currentChunkStart == null) {
                Logger.WriteLine($"{nameof(FinishCurrentChunk)}() called before {nameof(StartNewChunk)}()", LogType.Error);
                return;
            }

            AtOffset(0x2000 + _currentChunks * 0x08 + 0x04, curOffset => WriteUInt((uint) (curOffset - _currentChunkStart)));
            WriteToAlignTo(4);

            _currentChunks++;
            _currentChunkStart = null;
        }

        private int _currentChunks = 0;
        private long? _currentChunkStart = null;
    }
}
