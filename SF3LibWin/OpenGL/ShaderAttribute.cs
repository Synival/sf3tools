﻿using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class ShaderAttribute : BaseAttribute {
        public ShaderAttribute(int location, int elements, ActiveAttribType type, string name)
        : base(elements, type, name) {
            Location = location;
        }

        public int Location { get; }

        public bool IsAssignable(VBO_Attribute vboAttr)
            => vboAttr != null && Elements == vboAttr.Elements && Type == vboAttr.Type;
    }
}
