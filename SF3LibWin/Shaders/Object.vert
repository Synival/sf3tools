#version 330 core

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 color;
layout (location = 2) in vec3 glow;
layout (location = 3) in vec3 normal;

layout (location = 4) in vec2 texCoordAtlas;
layout (location = 5) in vec2 texCoordTerrainType;
layout (location = 6) in vec2 texCoordEventID;

out vec3 colorFrag;
out vec3 glowFrag;
out float lightingFrag;

out vec2 texCoordAtlasFrag;
out vec2 texCoordTerrainTypeFrag;
out vec2 texCoordEventIDFrag;

void main() {
    gl_Position   = projection * view * model * vec4(position, 1.0);
    colorFrag     = color;
    glowFrag      = glow;
    lightingFrag  = normal.x + normal.z + 0.25;

    texCoordAtlasFrag       = texCoordAtlas;
    texCoordTerrainTypeFrag = texCoordTerrainType;
    texCoordEventIDFrag     = texCoordEventID;
}
