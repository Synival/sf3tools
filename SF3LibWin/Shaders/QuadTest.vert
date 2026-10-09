#version 330 core

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 allVertices0;
layout (location = 2) in vec3 allVertices1;
layout (location = 3) in vec3 allVertices2;
layout (location = 4) in vec3 allVertices3;

layout (location = 5) in vec2 texCoordAtlasUV0;
layout (location = 6) in vec2 texCoordAtlasUV1;
layout (location = 7) in vec2 texCoordAtlasUV2;
layout (location = 8) in vec2 texCoordAtlasUV3;

noperspective out vec2 relativePos2DFrag;
flat out vec2 edgeBottomFrag;
flat out vec2 edgeLeftFrag;
flat out vec2 quadDistortionFrag;
flat out float coeffAFrag;

flat out vec2 texCoordAtlasUVFrag[4];

vec2 projectTo2D(vec3 pos, mat4 mvp) {
    vec4 clip = mvp * vec4(pos, 1.0);
    return clip.xy / clip.w;
}

float cross2D(vec2 a, vec2 b) {
    return a.x * b.y - a.y * b.x;
}

void main() {
    mat4 mvp = projection * view * model;
    gl_Position = mvp * vec4(position, 1.0);

    vec2 allVertices2D0 = projectTo2D(allVertices0, mvp);
    vec2 allVertices2D1 = projectTo2D(allVertices1, mvp);
    vec2 allVertices2D2 = projectTo2D(allVertices2, mvp);
    vec2 allVertices2D3 = projectTo2D(allVertices3, mvp);

    relativePos2DFrag  = (gl_Position.xy / gl_Position.w) - allVertices2D0;
    edgeBottomFrag     = allVertices2D1 - allVertices2D0; // BR - BL
    edgeLeftFrag       = allVertices2D3 - allVertices2D0; // TL - BL
    quadDistortionFrag = allVertices2D0 - allVertices2D1 + allVertices2D2 - allVertices2D3; // BL - BR + TR - TL

    coeffAFrag         = cross2D(quadDistortionFrag, edgeBottomFrag);

    texCoordAtlasUVFrag[0] = texCoordAtlasUV0;
    texCoordAtlasUVFrag[1] = texCoordAtlasUV1;
    texCoordAtlasUVFrag[2] = texCoordAtlasUV2;
    texCoordAtlasUVFrag[3] = texCoordAtlasUV3;
}
