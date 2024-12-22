using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class BaseAttribute {
        public BaseAttribute(int elements, ActiveAttribType type, string name) {
            Elements      = elements;
            Type          = type;
            PointerType   = GetTypePointerType(type);
            TypeElements  = GetTypeElements(type);
            TypeSize      = GetTypeSize(type);
            Name          = name;
            SizeInBytes   = elements * TypeSize;
        }

        public bool IsAssignable(BaseAttribute vboAttr)
            => vboAttr != null && Elements == vboAttr.Elements && Type == vboAttr.Type;

        public static VertexAttribPointerType GetTypePointerType(ActiveAttribType type) {
            switch (type) {
                case ActiveAttribType.Float:
                case ActiveAttribType.FloatVec2:
                case ActiveAttribType.FloatVec3:
                case ActiveAttribType.FloatVec4:
                    return VertexAttribPointerType.Float;

                default:
                    throw new NotImplementedException(nameof(type) + ": " + type.GetType().Name + "." + type.ToString());
            }
        }

        public static int GetTypeElements(ActiveAttribType type) {
            switch (type) {
                case ActiveAttribType.Float:     return 1;
                case ActiveAttribType.FloatVec2: return 2;
                case ActiveAttribType.FloatVec3: return 3;
                case ActiveAttribType.FloatVec4: return 4;

                default:
                    throw new NotImplementedException(nameof(type) + ": " + type.GetType().Name + "." + type.ToString());
            }
        }

        public static int GetPointerTypeSize(VertexAttribPointerType type) {
            switch (type) {
                case VertexAttribPointerType.Float: return sizeof(float);
                default:
                    throw new NotImplementedException(nameof(type) + ": " + type.GetType().Name + "." + type.ToString());
            }
        }

        public static int GetTypeSize(ActiveAttribType type)
            => GetPointerTypeSize(GetTypePointerType(type)) * GetTypeElements(type);

        public int Elements { get; }
        public ActiveAttribType Type { get; }
        public VertexAttribPointerType PointerType { get; }
        public int TypeElements { get; }
        public int TypeSize { get; }
        public string Name { get; }
        public int SizeInBytes { get; }
    }
}
