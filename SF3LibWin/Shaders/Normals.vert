#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat3 normalMatrix;

out vec3 normalFrag;

void main() {
    gl_Position = projection * view * model * vec4(position, 1.0);

    float prevLength = length(normal);
    vec3 modelNormal = normalize(normalMatrix * normal) * prevLength;

    normalFrag = modelNormal * vec3(1.0, 0.0, 1.0) + 0.5;
}
