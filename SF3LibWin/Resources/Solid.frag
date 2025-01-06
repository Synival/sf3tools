#version 330 core

in vec3 colorFrag;
out vec4 FragColor;

void main() {
    FragColor = vec4(colorFrag, 1.0);
}
