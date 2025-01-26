#version 330 core

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat3 normalMatrix;
uniform vec3 lightPosition;
uniform sampler2D textureLighting;
uniform bool useNewLighting;

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec3 glow;
layout (location = 3) in vec3 normal;

layout (location = 4) in vec2 texCoordAtlas;
layout (location = 5) in vec2 texCoordTerrainTypes;
layout (location = 6) in vec2 texCoordEventIDs;

layout (location = 7) in float applyLighting;

out vec4 colorFrag;
out vec3 glowFrag;
out vec4 lightColorFrag;

out vec2 texCoordAtlasFrag;
out vec2 texCoordTerrainTypesFrag;
out vec2 texCoordEventIDsFrag;

void main() {
    gl_Position   = projection * view * model * vec4(position, 1.0);
    colorFrag     = color;
    glowFrag      = glow;

    float prevLength = length(normal);
    vec3 modelNormal = normalize(normalMatrix * normal) * prevLength;
    float normalLightDot = dot(modelNormal, lightPosition);

    float lighting = !useNewLighting
        // Scenario 1 uses a straight-forward lighting method where the dot product directly references the index of
        // the color palette to use.
        ? (normalLightDot * 0.4995 + 0.4995)
        // Scenario 2 uses this odd exponential function instead, usually at pitch 0xB308. With this formula:
        // - any polygon not facing the light source (90 degrees or more) always uses the darkest color
        // - the color referenced used intentionally overflows, wrapping once
        // - a wider range of colors is used when the light is directly overhead
        // - the color used changes more rapidly the less direct the light is due to the exponent
        : (normalLightDot < 0) ? 0 : (0.666 * normalLightDot + 0.334 * pow(normalLightDot, 12)) * 1.999;

    lightColorFrag           = (applyLighting > 0.50) ? vec4(clamp(texture(textureLighting, vec2(0, lighting)).xyz - 0.5, -0.5, 0.5), 0) : vec4(0, 0, 0, 0);
    texCoordAtlasFrag        = texCoordAtlas;
    texCoordTerrainTypesFrag = texCoordTerrainTypes;
    texCoordEventIDsFrag     = texCoordEventIDs;
}
