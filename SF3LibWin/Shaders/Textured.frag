#version 330 core

uniform sampler2D texture0;

in vec3 colorFrag;
in vec2 texCoordFrag;

out vec4 FragColor;

void main() {
    FragColor = texture(texture0, texCoordFrag) * vec4(colorFrag, 1.0);
}
