#version 330 core

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat3 normalMatrix;
uniform vec3 lightPosition;
uniform sampler2D textureLighting;
uniform int lightingMode;
uniform bool smoothLighting;

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec3 glow;
layout (location = 3) in vec3 normal;

layout (location = 4) in vec2 texCoordAtlas;
layout (location = 5) in vec2 texCoordOverlay1;
layout (location = 6) in vec2 texCoordOverlay2;

layout (location = 7) in float applyLighting;
layout (location = 8) in float mesh;

out vec4 colorFrag;
out vec3 glowFrag;
out vec4 lightColorFrag;
out float meshFrag;

out vec2 texCoordAtlasFrag;
out vec2 texCoordOverlay1Frag;
out vec2 texCoordOverlay2Frag;

void main() {
    gl_Position = projection * view * model * vec4(position, 1.0);
    colorFrag   = color;
    glowFrag    = glow;
    meshFrag    = mesh;

    // Modify the normal based on the normal matrix.
    // Preserve the length of the normal for in-game accuracy.
    float prevLength = length(normal);
    vec3 modelNormal = normalize(normalMatrix * normal) * prevLength;
    float normalLightDot = dot(modelNormal, lightPosition);

    float lighting =
        (lightingMode == 0) ? 0.0f :
        // Scenario 1 always uses a straight-forward lighting method where the dot product directly references the index of
        // the color palette to use.
        (lightingMode == 1) ? (normalLightDot * 0.5f + 0.5f) :
        // Reverse-engineered function. Don't ask me why it is what it is!
        ((normalLightDot < 0) ? 0.0f : (atan(normalLightDot, sqrt(1.0f - normalLightDot * normalLightDot)) * 1.27323954477 /* <-- 4/pi */ + 2.0f));

    lighting = (smoothLighting ? clamp(lighting, 0.0, 0.96875) : floor(lighting * 32.0f) / 32.0f) + 0.015625;

    lightColorFrag       = (lightingMode != 0 && applyLighting > 0.50) ? vec4(clamp(texture(textureLighting, vec2(0, lighting)).xyz - 0.5, -0.5, 0.5), 0) : vec4(0, 0, 0, 0);
    texCoordAtlasFrag    = texCoordAtlas;
    texCoordOverlay1Frag = texCoordOverlay1;
    texCoordOverlay2Frag = texCoordOverlay2;
}
