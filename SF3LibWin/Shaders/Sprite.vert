#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec2 texCoord0;
layout (location = 3) in float width;
layout (location = 4) in float isRightVertex;
layout (location = 5) in float directions;
layout (location = 6) in float isFlippable;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform float direction;
uniform float cameraDistAdjust;

out vec4 colorFrag;
out vec2 texCoord0Frag;

void main() {
    // Render the sprite as if it's closer to the camera by 0.5 units.
    // This will appear in the same place, but write to the depth buffer differently.
    vec4 viewPos = view * model * vec4(position, 1.0);
    viewPos.z += cameraDistAdjust;
    gl_Position = projection * viewPos;

    colorFrag = color;

    // Conversions for simpler math.
    int directionsInt = int(round(directions));
    bool flippable    = (isFlippable >= 0.5f);
    bool rightVertex  = (isRightVertex >= 0.5f);

    // Determine if the sprite will need to be flipped, which is if it's 'north' or beyond.
    // (Direction is 0.00=south, 0.25=east, 0.50=north, 0.75=west)
    bool flipped = flippable ? (direction >= 0.5f) : false;

    // Adjust angle to snap to 22.5 degree orientations.
    float angleSnap =
        (directionsInt ==  8) ? -0.0625f :
        (directionsInt == 10) ? -0.0500f : 0.00f;

    // Determine the frame to use based on the angle, number of directions, and flippability.
    // Flippable sprites have half as many frames, so account for that.
    int numFrames = flippable ? (directionsInt / 2) : directionsInt;
    int frame = int(clamp(round(((flipped ? (1.0f - direction) : direction) + angleSnap) * directionsInt), 0, numFrames - 1));

    // Determine final U coordinate based on which vertex this is and the 'flipped' state.
    int uOffset = (flipped ^^ rightVertex) ? 1 : 0;

    // Calculations complete -- finally output the texture coordinate.
    texCoord0Frag = texCoord0 + vec2((frame + uOffset) * width, 0);
}
