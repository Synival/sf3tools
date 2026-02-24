#version 330 core

uniform sampler2D texture0;
uniform bool colorize;
uniform vec4 color;

in vec4 colorFrag;
in vec2 texCoord0Frag;

out vec4 FragColor;

void main() {
    vec4 texColor = texture(texture0, texCoord0Frag) * colorFrag;
    if (texColor.a < 0.001)
        discard;

    FragColor = colorize ? color : texColor;
}
