#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec2 texCoord0;
layout (location = 3) in vec2 texCoord1;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec4 colorFrag;
out vec2 texCoord0Frag;
out vec2 texCoord1Frag;

void main() {
    gl_Position = projection * view * model * vec4(position, 1.0);
    colorFrag = color;
    texCoord0Frag = texCoord0;
    texCoord1Frag = texCoord1;
}
