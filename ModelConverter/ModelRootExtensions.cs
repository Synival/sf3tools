using System;
using System.Runtime.InteropServices;
using CommonLib.Extensions;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;

namespace ModelConverter {
    public static class ModelRootExtensions {
        public static Accessor CreateStandardAccessor(this ModelRoot modelRoot, string name, BufferView bufferView, int offset, int count, DimensionType dimensionType, EncodingType encodingType) {
            var accessor = modelRoot.CreateAccessor(name);
            var attrFormat = new AttributeFormat(dimensionType, encodingType, nrm: false);
            accessor.SetData(bufferView, offset, count, attrFormat);
            accessor.UpdateBounds();
            return accessor;
        }

        public static Accessor CreateUShortAccessor(this ModelRoot modelRoot, string name, BufferView bufferView, int offset, int count)
            => CreateStandardAccessor(modelRoot, name, bufferView, offset, count, DimensionType.SCALAR, EncodingType.UNSIGNED_SHORT);

        public static Accessor CreateVector2Accessor(this ModelRoot modelRoot, string name, BufferView bufferView, int offset, int count)
            => CreateStandardAccessor(modelRoot, name, bufferView, offset, count, DimensionType.VEC2, EncodingType.FLOAT);

        public static Accessor CreateVector3Accessor(this ModelRoot modelRoot, string name, BufferView bufferView, int offset, int count)
            => CreateStandardAccessor(modelRoot, name, bufferView, offset, count, DimensionType.VEC3, EncodingType.FLOAT);

        public static Accessor CreateVector4Accessor(this ModelRoot modelRoot, string name, BufferView bufferView, int offset, int count)
            => CreateStandardAccessor(modelRoot, name, bufferView, offset, count, DimensionType.VEC4, EncodingType.FLOAT);

        public static Accessor CreateTriangeIndiciesAccessor(this ModelRoot modelRoot, string name, ushort[,] data) {
            var bufferView = modelRoot.CreateBufferView(data.Length * sizeof(ushort), 0, BufferMode.ELEMENT_ARRAY_BUFFER);
            MemoryMarshal.Cast<ushort, byte>(data.To1DArray().AsSpan()).CopyTo(bufferView.Content.AsSpan());
            return CreateStandardAccessor(modelRoot, name, bufferView, 0, data.Length, DimensionType.SCALAR, EncodingType.UNSIGNED_SHORT);
        }
    }
}
