#version 330 core

uniform sampler2D texture0;

in vec3 colorFrag;
in vec2 texCoord0Frag;
in vec3 glowFrag;

out vec4 FragColor;

void main() {
    FragColor = texture(texture0, texCoord0Frag) * vec4(colorFrag, 1.0) + vec4(glowFrag, 0.0);
}
