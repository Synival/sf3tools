#version 330 core

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat3 normalMatrix;
uniform vec3 lightPosition;
uniform sampler2D textureLighting;
uniform int lightingMode;
uniform bool smoothLighting;
uniform vec2 pixelSize;

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec3 glow;
layout (location = 3) in vec3 normal;

layout (location = 4) in vec2 texCoordOverlay1;
layout (location = 5) in vec2 texCoordOverlay2;

layout (location = 6) in float applyLighting;
layout (location = 7) in float mesh;

// Position of all vertices. These should be identical across all four vertices for the quad.
layout (location = 8)  in vec3 allVertices0;
layout (location = 9)  in vec3 allVertices1;
layout (location = 10) in vec3 allVertices2;
layout (location = 11) in vec3 allVertices3;

// UV coordinates of all vertices. These should be identical across all four vertices for the quad.
layout (location = 12) in vec2 texCoordAtlasUV0;
layout (location = 13) in vec2 texCoordAtlasUV1;
layout (location = 14) in vec2 texCoordAtlasUV2;
layout (location = 15) in vec2 texCoordAtlasUV3;

// Linearly-interpreted position of the current pixel relative to allVertices0.
noperspective out vec2 relativePos2DFrag;

// Pre-calculations for equations in the fragment shader.
flat out vec2 edgeBottomFrag;
flat out vec2 edgeLeftFrag;
flat out vec2 quadDistortionFrag;
flat out float coeffAFrag;
flat out int interpolateLinearlyFrag;

// UV coordinates of the entire quad passed along to the fragment shader.
flat out vec2 texCoordAtlasUVFrag[4];
flat out vec2 uvClampULFrag;
flat out vec2 uvClampBRFrag;

out vec4 colorFrag;
out vec3 glowFrag;
out vec4 lightColorFrag;
flat out int meshFrag;

out vec2 texCoordOverlay1Frag;
out vec2 texCoordOverlay2Frag;

// Converts a 3D model coordinate to a 2D screen coordinate.
vec2 projectTo2D(vec3 pos, mat4 mvp) {
    vec4 clip = mvp * vec4(pos, 1.0);
    return clip.xy / clip.w;
}

// Calculates the 2D cross product.
float cross2D(vec2 a, vec2 b) {
    return a.x * b.y - a.y * b.x;
}

void main() {
    mat4 mvp = projection * view * model;
    gl_Position = mvp * vec4(position, 1.0);
    colorFrag   = color;
    glowFrag    = glow;
    meshFrag    = mesh > 0.5 ? 1 : 0;

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

    lightColorFrag = (lightingMode != 0 && applyLighting > 0.50)
        ? vec4(clamp(texture(textureLighting, vec2(0.0, lighting)).xyz - 0.5, -0.5, 0.5), 0.0)
        : vec4(0.0);

    texCoordOverlay1Frag = texCoordOverlay1;
    texCoordOverlay2Frag = texCoordOverlay2;

    // Get the position of the entire quad in screen coordinates.
    vec2 allVertices2D0 = projectTo2D(allVertices0, mvp);
    vec2 allVertices2D1 = projectTo2D(allVertices1, mvp);
    vec2 allVertices2D2 = projectTo2D(allVertices2, mvp);
    vec2 allVertices2D3 = projectTo2D(allVertices3, mvp);

    // Calculations to pass along to the fragment shader.
    relativePos2DFrag  = (gl_Position.xy / gl_Position.w) - allVertices2D0;
    edgeBottomFrag     = allVertices2D1 - allVertices2D0; // BR - BL
    edgeLeftFrag       = allVertices2D3 - allVertices2D0; // TL - BL
    quadDistortionFrag = allVertices2D0 - allVertices2D1 + allVertices2D2 - allVertices2D3; // BL - BR + TR - TL
    coeffAFrag         = cross2D(quadDistortionFrag, edgeBottomFrag);
    interpolateLinearlyFrag = (abs(coeffAFrag) < 1e-5) ? 1 : 0;

    // Pass UV coordinates along to the fragment shader.
    texCoordAtlasUVFrag[0] = texCoordAtlasUV0;
    texCoordAtlasUVFrag[1] = texCoordAtlasUV1;
    texCoordAtlasUVFrag[2] = texCoordAtlasUV2;
    texCoordAtlasUVFrag[3] = texCoordAtlasUV3;

    // Send UV coordinate clamp values to prevent seams that can occur with linear filtering.
    vec2 texCoordUL = vec2(min(texCoordAtlasUV0.x, texCoordAtlasUV2.x), min(texCoordAtlasUV0.y, texCoordAtlasUV2.y));
    vec2 texCoordBR = vec2(max(texCoordAtlasUV0.x, texCoordAtlasUV2.x), max(texCoordAtlasUV0.y, texCoordAtlasUV2.y));
    vec2 halfPixelSize = pixelSize * 0.51; 
    uvClampULFrag = texCoordUL + halfPixelSize;
    uvClampBRFrag = texCoordBR - halfPixelSize;
}
