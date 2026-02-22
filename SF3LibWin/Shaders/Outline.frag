#version 330 core

uniform sampler2D texture0;
uniform vec3 color;
uniform bool alwaysShow;

in vec2 texCoord0Frag;

out vec4 FragColor;

void main() {
    if (!alwaysShow && texture(texture0, texCoord0Frag).a < 0.001)
        discard;

    FragColor = vec4(color, 1);
}
