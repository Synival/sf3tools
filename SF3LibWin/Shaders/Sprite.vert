#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec2 texCoord0;
layout (location = 3) in float width;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform float direction;

out vec4 colorFrag;
out vec2 texCoord0Frag;

void main() {
    gl_Position = projection * view * model * vec4(position, 1.0);
    colorFrag = color;

    // Resulting angle: 0.00=south, 0.25=east, 0.50=north, 0.75=west
    float angle = direction;

    // Assume 8 directions, non-flippable.
    // Target angles:
    //     0.0625: south-southeast
    //     0.1875: east-southeast
    //     0.3125: east-northeast
    //     0.4375: north-northeast
    //     0.5625: north-northwest
    //     0.6875: west-northwest
    //     0.8125: west-southwest
    //     0.9375: south-southwest

    // Adjust angle to snap to 22.5 degree orientations.
    angle = mod(angle - 0.0625f, 1.0f);

    float direction = mod(round(angle * 8.0f), 8.0f);
    texCoord0Frag = texCoord0 + vec2(direction, 0) * width;
}
