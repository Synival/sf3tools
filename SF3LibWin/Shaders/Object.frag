#version 330 core

uniform sampler2D textureAtlas;

in vec3 colorFrag;
in vec2 texCoordAtlasFrag;
in vec3 glowFrag;
in float lightingFrag;

out vec4 FragColor;

void main() {
    FragColor =
        texture(textureAtlas, texCoordAtlasFrag) * vec4(colorFrag, 1.0)
      + vec4(glowFrag, 0.0)
      + vec4(vec3(1.0, 1.0, 0.5) * lightingFrag * 0.2, 0);
}
