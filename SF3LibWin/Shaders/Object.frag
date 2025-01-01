#version 330 core

uniform sampler2D texture0;

in vec3 colorFrag;
in vec2 texCoord0Frag;
in vec3 glowFrag;
in float lightingFrag;

out vec4 FragColor;

void main() {
    FragColor =
        texture(texture0, texCoord0Frag) * vec4(colorFrag, 1.0)
      + vec4(glowFrag, 0.0)
      + vec4(vec3(1.0, 1.0, 0.5) * lightingFrag * 0.2, 0);
}
