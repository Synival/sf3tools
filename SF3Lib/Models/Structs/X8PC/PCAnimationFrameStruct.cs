using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class PCAnimationFrameStruct : IStruct, ISGL_ModelInstanceCollection {
        public PCAnimationFrameStruct(IByteData data, string name, int id, float frame, PolyChar polyChar) {
            Data     = data;
            Name     = name;
            ID       = id;
            Frame    = frame;
            PolyChar = polyChar;

            _models = polyChar.BoneTable
                .SelectMany(x => x
                    .Where(y => !y.ForceTransparency.HasValue)
                    .ToArray()
                ).ToArray();
        }

        private struct KeyframeInfo {
            public int IndexA, IndexB;
            public float Interp;
            public override string ToString() => $"({IndexA}, {IndexB}) ({Interp})";
        }

        private KeyframeInfo GetKeyframe<T>(T[] list, Func<T, int> frameGetter) {
            var frame = Frame;
            int max = list.Length;
            for (int i = 0; i < max; i++) {
                var element = list[i];
                var f = frameGetter(element);
                if (frame >= f || i == max - 1) {
                    if (i == max - 1)
                        return new KeyframeInfo() { IndexA = i, IndexB = i, Interp = 0.0f };
                    else {
                        var nextF = frameGetter(list[i + 1]);
                        return new KeyframeInfo() { IndexA = i, IndexB = i + 1, Interp = (nextF - f) / (float) (frame - f) };
                    }
                }
            }
            return new KeyframeInfo() { IndexA = 0, IndexB = 0, Interp = 0.0f };
        }

        public IByteData Data { get; }

        [TableViewModelColumn(displayOrder: -2, displayFormat: "000.0")]
        [BulkCopy]
        public float Frame { get; }

        [TableViewModelColumn(displayOrder: -1, minWidth: 120)]
        [BulkCopy]
        public string Name { get; }

        public ISGL_ModelInstance GetModelInstance(int id) => _models.FirstOrDefault(x => x.ModelInstanceID == id);
        public IEnumerator<ISGL_ModelInstance> GetEnumerator() => _models.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public PolyChar PolyChar { get; }

        private ISGL_ModelInstance[] _models;

        public int ID { get; }
        public int Address => 0; /* N/A */
        public int Size => 0; /* N/A */
    }
}
