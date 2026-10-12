#version 330 core

// QuadRenderer
// ------------
// Template shader that performs Saturn-like bilinear interpolation for texturing quads.
//
// To use, the following requirements must be met:
//
// 1. Vertices cannot be shared between multiple quads, so you'll need to duplicate them
// 2. All four vertices must be supplied to each vertex as allVertices0...3
// 3. All four UV coordinates must be supplied to each vertex as texCoordAtlasUV0...3 *
//
// * Why 'Atlas'? Because this originally came from an atlas and I'm too lazy to undo it.

// Model, view, and project matrix (could be a single matrix some day)
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

// Position of the current vertex.
layout (location = 0) in vec3 position;

// Position of all vertices. These should be identical across all four vertices for the quad.
layout (location = 1) in vec3 allVertices0;
layout (location = 2) in vec3 allVertices1;
layout (location = 3) in vec3 allVertices2;
layout (location = 4) in vec3 allVertices3;

// UV coordinates of all vertices. These should be identical across all four vertices for the quad.
// NOTE: The 'Atlas' part is just because this originally was only used for texture atlases. It's a
//       bit vestigial and will be renamed in due time.
layout (location = 5) in vec2 texCoordAtlasUV0;
layout (location = 6) in vec2 texCoordAtlasUV1;
layout (location = 7) in vec2 texCoordAtlasUV2;
layout (location = 8) in vec2 texCoordAtlasUV3;

// Dimensions of individual pixels used for clamping UV coordinates.
layout (location = 9) in vec2 pixelSize;

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

    // Pass UV coordinate salong to the fragment shader.
    texCoordAtlasUVFrag[0] = texCoordAtlasUV0;
    texCoordAtlasUVFrag[1] = texCoordAtlasUV1;
    texCoordAtlasUVFrag[2] = texCoordAtlasUV2;
    texCoordAtlasUVFrag[3] = texCoordAtlasUV3;

    // Send UV coordinate clamp values to prevent seams that can occur with linear filtering.
    vec2 texCoordUL = vec2(min(texCoordAtlasUV0.x, texCoordAtlasUV2.x), min(texCoordAtlasUV0.y, texCoordAtlasUV2.y));
    vec2 texCoordBR = vec2(max(texCoordAtlasUV0.x, texCoordAtlasUV2.x), max(texCoordAtlasUV0.y, texCoordAtlasUV2.y));
    vec2 halfPixelSize = pixelSize * 0.51; // A little more than 0.5 just in case...
    uvClampULFrag = texCoordUL + halfPixelSize;
    uvClampBRFrag = texCoordBR - halfPixelSize;
}
