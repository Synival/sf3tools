#version 330 core

uniform sampler2D textureAtlas;
uniform vec4 color;
uniform bool alwaysShow;

in vec2 texCoordAtlasFrag;

out vec4 FragColor;

void main() {
    if (!alwaysShow && texture(textureAtlas, texCoordAtlasFrag).a < 0.001)
        discard;

    FragColor = color;
}
