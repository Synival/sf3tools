#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoord0;

uniform mat4 view;

out vec2 texCoord0Frag;

void main() {
    gl_Position = vec4(position, 1.0);
    texCoord0Frag = texCoord0;
}
