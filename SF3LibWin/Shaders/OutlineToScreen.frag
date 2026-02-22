#version 330 core

uniform sampler2D texture0;

in vec2 texCoord0Frag;

out vec4 FragColor;

void main() {
    vec4 color = texture(texture0, texCoord0Frag);
    float adjustedAlpha = pow(color.a, 0.50f);
    float adjustedIntensity = adjustedAlpha / color.a;
    FragColor = vec4(color.rgb * adjustedIntensity, adjustedAlpha);
}
