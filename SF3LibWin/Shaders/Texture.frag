#version 330 core

uniform sampler2D texture0;

in vec4 colorFrag;
in vec2 texCoord0Frag;
in vec3 glowFrag;

out vec4 FragColor;

void main() {
    vec4 texColor = texture(texture0, texCoord0Frag) * colorFrag + vec4(glowFrag, 0.0);
    if (texColor.a < 0.001)
        discard;

    FragColor = texColor;
}
