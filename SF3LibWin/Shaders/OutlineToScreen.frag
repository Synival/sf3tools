#version 330 core

uniform sampler2D texture0;

in vec2 texCoord0Frag;

out vec4 FragColor;

void main() {
    vec4 color = texture(texture0, texCoord0Frag);
    FragColor = vec4(color.rgb, pow(color.a, 0.25f));
}
