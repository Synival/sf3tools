#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoord1;
layout (location = 2) in vec4 color;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 texCoord1Frag;
out float alphaFrag;

void main() {
    gl_Position = projection * view * model * vec4(position, 1.0);
    texCoord1Frag = texCoord1;
    alphaFrag = color.a;
}
