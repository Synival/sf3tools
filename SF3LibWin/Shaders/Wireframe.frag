#version 330 core

uniform sampler2D texture1;

in vec2 texCoord1Frag;
in float alphaFrag;

out vec4 FragColor;

void main() {
    vec4 texColor = texture(texture1, texCoord1Frag) * vec4(1, 1, 1, alphaFrag);
    if (texColor.a < 0.001)
        discard;

    FragColor = texColor;
}
