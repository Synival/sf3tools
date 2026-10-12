#version 330 core

uniform sampler2D textureAtlas;
uniform sampler2D textureOverlay1;
uniform sampler2D textureOverlay2;

in vec4 colorFrag;
in vec3 glowFrag;
in vec4 lightColorFrag;
flat in int meshFrag;

in vec2 texCoordOverlay1Frag;
in vec2 texCoordOverlay2Frag;

// Linearly-interpreted position of the current pixel relative to allVertices0.
noperspective in vec2 relativePos2DFrag;

// Pre-calculations from the vertex shader for equations here in the fragment shader.
flat in vec2 edgeBottomFrag;
flat in vec2 edgeLeftFrag;
flat in vec2 quadDistortionFrag;
flat in float coeffAFrag;
flat in int interpolateLinearlyFrag;

// UV coordinates of the entire quad passed along to the fragment shader.
flat in vec2 texCoordAtlasUVFrag[4];
flat in vec2 uvClampULFrag;
flat in vec2 uvClampBRFrag;

// Resulting output color.
out vec4 FragColor;

// Calculates the 2D cross product.
float cross2D(vec2 a, vec2 b) {
    return a.x * b.y - a.y * b.x;
}

// Calculates the Y coordinate based on the X coordinate.
float calcY(float x) {
    vec2 denomV = edgeLeftFrag + x * quadDistortionFrag;
    vec2 denomVAbs = abs(denomV);
    
    float denom;
    float denomMax;
    float num;

    // Each branch is mathematically equivalent, but the larger denominator produces more accurate results // due to
    // floating point imprecisions.
    if (denomVAbs.x > denomVAbs.y) {
        denom    = denomV.x;
        denomMax = denomVAbs.x;
        num      = relativePos2DFrag.x - x * edgeBottomFrag.x;
    }
    else {
        denom    = denomV.y;
        denomMax = denomVAbs.y;
        num      = relativePos2DFrag.y - x * edgeBottomFrag.y;
    }

    return (denomMax < 1e-5) ? 0.5 : (num / denom);
}

// Inverse bilinear interpolation of the current screen coordinate within the quad coordinates to determine the
// position within the quad in range (0, 0) to (1, 1).
vec2 getPosInQuad() {
    // Position is determined using inverse bilinear interpolation.
    // Several variables are pre-calculated in the vertex shader and passed along.
    float a = coeffAFrag;
    float b = cross2D(relativePos2DFrag, quadDistortionFrag) + cross2D(edgeLeftFrag, edgeBottomFrag);
    float c = cross2D(relativePos2DFrag, edgeLeftFrag);

    // Determine 'u' and 'v' using a lot of fancy checks.
    float x = 0.0;
    float y = 0.0;
    
    // Parallelograms and top/bottom-collapsed triangles (a == 0)
    if (interpolateLinearlyFrag > 0) {
        x = (abs(b) < 1e-5)
            ? 0.5             // Utterly degenerate point; safely sample center
            : (-c * (1.0/b)); // Linear root
        y = calcY(x);
    } 
    // Normal and distorted quads
    else {
        float discriminant = max(b*b - 4.0*a*c, 0.0);

        // Calculate the discriminant in a way that matches the sign of b,
        // so the signs of the numbers added are always the same, avoiding cancellation.
        float signB = (b >= 0.0) ? 1.0 : -1.0;
        float discriminantSqrt = signB * sqrt(discriminant);

        // Full quadradic formula without dividing by 'a'.
        float q = -0.5 * (b + discriminantSqrt);

        x = q / a;
        y = calcY(x);

        // If (x, y) does not land inside the quad (with a tiny epsilon for edge floating point inaccuracy), use the other root.
        if (!(x >= -0.001 && x <= 1.001 && y >= -0.001 && y <= 1.001)) {
            // Fancy-shmancy way to calculate the other root for 'u' by using formulas by a
            // 16th century gentleman named "Vieta".
            x = c / q;
            y = calcY(x);
        }
    }

    // Return our (x, y) value, clamped within bounds just in case.
    return clamp(vec2(x, y), 0.0, 1.0);
}

void main() {
    if (meshFrag > 0.0) {
        ivec2 fragInt = ivec2(gl_FragCoord.xy) >> 1;
        if (((fragInt.x + fragInt.y) & 1) == 0) discard;
    }

    vec2 posInQuad = getPosInQuad();

    vec2 uvCoord = clamp(mix(
        mix(texCoordAtlasUVFrag[0], texCoordAtlasUVFrag[1], posInQuad.x),
        mix(texCoordAtlasUVFrag[3], texCoordAtlasUVFrag[2], posInQuad.x),
        posInQuad.y
    ), uvClampULFrag, uvClampBRFrag);

    vec4 surfaceTex = ((texture(textureAtlas, uvCoord) * colorFrag) + lightColorFrag);
    surfaceTex = surfaceTex + vec4(glowFrag, 0.0);

    vec4 overlayTex =
        texture(textureOverlay1, texCoordOverlay1Frag) +
        texture(textureOverlay2, texCoordOverlay2Frag);

    vec4 compositeColor = vec4(mix(surfaceTex.rgb, overlayTex.rgb, overlayTex.a), surfaceTex.a + overlayTex.a * (1.0 - surfaceTex.a));
    if (compositeColor.a < 1e-5)
        discard;

    FragColor = compositeColor;
}
