#version 330 core

uniform sampler2D textureAtlas;
uniform sampler2D textureOverlay1;
uniform sampler2D textureOverlay2;

in vec4 colorFrag;
in vec3 glowFrag;
in vec4 lightColorFrag;
in float meshFrag;

in vec2 texCoordOverlay1Frag;
in vec2 texCoordOverlay2Frag;

noperspective in vec2 relativePos2DFrag;
flat in vec2 edgeBottomFrag;
flat in vec2 edgeLeftFrag;
flat in vec2 quadDistortionFrag;
flat in float coeffAFrag;

flat in vec2 texCoordAtlasUVFrag[4];

out vec4 FragColor;

float cross2D(vec2 a, vec2 b) {
    return a.x * b.y - a.y * b.x;
}

vec2 getPosInQuad() {
    // Position is determined using inverse bilinear interpolation.
    // Several variables are pre-calculated in the vertex shader and passed along.
    float a = coeffAFrag;
    float b = cross2D(relativePos2DFrag, quadDistortionFrag) + cross2D(edgeLeftFrag, edgeBottomFrag);
    float c = cross2D(relativePos2DFrag, edgeLeftFrag);

    // Determine 'u'. We need a lot of branching to account for possible triangles and
    // avoid division-by-zero errors.
    float u = 0.0;
    
    // Parallelograms and top/bottom-collapsed triangles (a == 0)
    if (abs(a) < 1e-5) {
        u = (abs(b) < 1e-5)
            ? 0.5       // Utterly degenerate point; safely sample center
            : (-c / b); // Linear root
    } 
    // Normal quads and left/right-collapsed triangles
    else {
        float discriminant = max(b*b - 4.0*a*c, 0.0);
        float discriminantSq = sqrt(discriminant);

        // Compute both standard quadratic possibilities and choose the closest to 0.5
        float u1 = (-b + discriminantSq) / (2.0 * a);
        float u2 = (-b - discriminantSq) / (2.0 * a);
        u = (abs(u1 - 0.5) < abs(u2 - 0.5)) ? u1 : u2;
    }

    // Determine 'v'.
    vec2 denomV = edgeLeftFrag + u * quadDistortionFrag;
    float v = (abs(denomV.x) > abs(denomV.y))
        ? ((abs(denomV.x) < 1e-5) ? 0.5 : (relativePos2DFrag.x - u * edgeBottomFrag.x) / denomV.x)
        : ((abs(denomV.y) < 1e-5) ? 0.5 : (relativePos2DFrag.y - u * edgeBottomFrag.y) / denomV.y);

    return clamp(vec2(u, v), 0.0, 1.0);
}

void main() {
    if (meshFrag > 0 && (int(gl_FragCoord.x / 2) + int(gl_FragCoord.y / 2)) % 2 == 0)
        discard;

    vec2 posInQuad = getPosInQuad();

    vec2 uvCoord = mix(
        mix(texCoordAtlasUVFrag[0], texCoordAtlasUVFrag[1], posInQuad.x),
        mix(texCoordAtlasUVFrag[3], texCoordAtlasUVFrag[2], posInQuad.x),
        posInQuad.y
    );

    vec4 surfaceTex = ((texture(textureAtlas, uvCoord) * colorFrag) + lightColorFrag);
    surfaceTex = surfaceTex + vec4(glowFrag, 0.0);

    vec4 overlayTex =
        texture(textureOverlay1, texCoordOverlay1Frag) +
        texture(textureOverlay2, texCoordOverlay2Frag);

    vec4 compositeColor = vec4(mix(surfaceTex.rgb, overlayTex.rgb, overlayTex.a), surfaceTex.a + overlayTex.a * (1.0 - surfaceTex.a));
    if (compositeColor.a < 0.001)
        discard;

    FragColor = compositeColor;
}
