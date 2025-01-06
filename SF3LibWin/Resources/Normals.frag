#version 330 core

in vec3 normalFrag;

out vec4 FragColor;

void main() {
    FragColor = vec4(normalFrag, 1.0);
}
