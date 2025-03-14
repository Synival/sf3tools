﻿using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using SF3.Types;
using SF3.Win.Extensions;
using static CommonLib.Extensions.ArrayExtensions;

namespace SF3.Win.OpenGL {
    public class Quad {
        private static readonly Vector4 c_white = new Vector4(1, 1, 1, 1);
        private static readonly Vector4[] c_allWhite = [c_white, c_white, c_white, c_white];

        public Quad(Vector3[] vertices) : this(vertices, null, TextureRotateType.NoRotation, TextureFlipType.NoFlip, c_white) { }
        public Quad(Vector3[] vertices, Vector4 color) : this(vertices, null, TextureRotateType.NoRotation, TextureFlipType.NoFlip, [color, color, color, color]) { }
        public Quad(Vector3[] vertices, Vector4[] colors) : this(vertices, null, TextureRotateType.NoRotation, TextureFlipType.NoFlip, colors) { }
        public Quad(Vector3[] vertices, TextureAnimation textureAnim, TextureRotateType rotate, TextureFlipType flip)
            : this(vertices, textureAnim, rotate, flip, c_allWhite) { }
        public Quad(Vector3[] vertices, TextureAnimation textureAnim, TextureRotateType rotate, TextureFlipType flip, Vector4 color)
            : this(vertices, textureAnim, rotate, flip, [color, color, color, color]) { }

        public Quad(Vector3[] vertices, TextureAnimation textureAnim, TextureRotateType rotate, TextureFlipType flip, Vector4[] colors) {
            if (vertices == null || vertices.Length != 4)
                throw new ArgumentException(nameof(vertices));
            if (colors == null || colors.Length != 4)
                throw new ArgumentException(nameof(colors));

            TextureAnim   = textureAnim;
            TextureRotate = rotate;
            TextureFlip   = flip;
            Center = vertices.Aggregate((a, b) => a + b) / vertices.Length;

            // Determine the largest dimension and consider that our scale.
            var minCoords = new float[] { vertices[0][0], vertices[0][1], vertices[0][2] };
            var maxCoords = new float[] { vertices[0][0], vertices[0][1], vertices[0][2] };
            for (var i = 0; i < 4; i++) {
                for (var j = 0; j < 3; j++) {
                    var value = vertices[i][j];
                    if (value < minCoords[j])
                        minCoords[j] = value;
                    if (value > maxCoords[j])
                        maxCoords[j] = value;
                }
            }
            var scale = Math.Max(Math.Max(maxCoords[2] - minCoords[2], maxCoords[1] - minCoords[1]), maxCoords[0] - minCoords[0]);

            // If the center of this quad isn't equal to the sum/average of its opposite corners, add a fifth vertex
            // that will divide it into 4 triangles rather than 2. Use a very lenient tolerance to prevent too many
            // mostly-float polygons from being subdivided.
            var sum1 = vertices[0] + vertices[2];
            var sum2 = vertices[1] + vertices[3];
            HasCenterVertex = !VerticesAreBasicallyEqual(sum1, sum2, scale / 16.0f);
            Vertices = HasCenterVertex ? 5 : 4;

            Attributes = [];
            AddAttribute(new PolyAttribute(1, OpenTK.Graphics.OpenGL.ActiveAttribType.FloatVec3, "position", 4,
                vertices.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3)));
            AddAttribute(new PolyAttribute(1, OpenTK.Graphics.OpenGL.ActiveAttribType.FloatVec4, "color", 4,
                colors.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 4)));
        }

        private bool VerticesAreBasicallyEqual(Vector3 lhs, Vector3 rhs, float tolerance) {
            if (lhs[0] > rhs[0] + tolerance || lhs[0] < rhs[0] - tolerance)
                return false;
            if (lhs[1] > rhs[1] + tolerance || lhs[1] < rhs[1] - tolerance)
                return false;
            if (lhs[2] > rhs[2] + tolerance || lhs[2] < rhs[2] - tolerance)
                return false;
            return true;
        }

        public void AddAttribute(PolyAttribute attr) {
            if (_attributesByName.ContainsKey(attr.Name))
                throw new ArgumentException(nameof(attr.Name));

            if (attr.Vertices != 4) {
                try {
                    throw new ArgumentException(nameof(attr.Vertices) + ": Is " + attr.Vertices + ", should be 4");
                }
                catch { }
                return;
            }

            if (HasCenterVertex) {
                // Produce a fifth center vertex attribute with an average of all 4 corners.
                var length2 = attr.Data.GetLength(1);
                var newData = new float[5, length2];
                for (var i = 0; i < 4; i++) {
                    for (var j = 0; j < length2; j++) {
                        newData[i, j] = attr.Data[i, j];
                        newData[4, j] += attr.Data[i, j];
                    }
                }
                for (var j = 0; j < length2; j++)
                    newData[4, j] /= 4.0f;

                var newAttr = new PolyAttribute(attr.Elements, attr.Type, attr.Name, 5, newData);
                Attributes.Add(newAttr);
                _attributesByName.Add(newAttr.Name, newAttr);
            }
            else {
                Attributes.Add(attr);
                _attributesByName.Add(attr.Name, attr);
            }
        }

        public PolyAttribute GetAttributeByName(string name)
            => _attributesByName.TryGetValue(name, out var value) ? value : null;

        public List<PolyAttribute> Attributes { get; }
        private Dictionary<string, PolyAttribute> _attributesByName = [];

        public TextureAnimation TextureAnim { get; }

        public TextureRotateType TextureRotate { get; }
        public TextureFlipType TextureFlip { get; }

        public bool HasCenterVertex { get; }
        public int Vertices { get; }

        public Vector3 Center { get; }
    }
}
