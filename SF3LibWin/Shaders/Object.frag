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

noperspective in vec2 relativePos2DFrag;
flat in vec2 edgeBottomFrag;
flat in vec2 edgeLeftFrag;
flat in vec2 quadDistortionFrag;
flat in float coeffAFrag;
flat in int interpolateLinearlyFrag;

flat in vec2 texCoordAtlasUVFrag[4];

out vec4 FragColor;

float cross2D(vec2 a, vec2 b) {
    return a.x * b.y - a.y * b.x;
}

float calcV(float u) {
    vec2 denomV = edgeLeftFrag + u * quadDistortionFrag;
    vec2 denomVAbs = abs(denomV);
    
    float denom;
    float denomMax;
    float num;

    if (denomVAbs.x > denomVAbs.y) {
        denom    = denomV.x;
        denomMax = denomVAbs.x;
        num      = relativePos2DFrag.x - u * edgeBottomFrag.x;
    }
    else {
        denom    = denomV.y;
        denomMax = denomVAbs.y;
        num      = relativePos2DFrag.y - u * edgeBottomFrag.y;
    }
    
    return (denomMax < 1e-5) ? 0.5 : (num / denom);
}

vec2 getPosInQuad() {
    // Position is determined using inverse bilinear interpolation.
    // Several variables are pre-calculated in the vertex shader and passed along.
    float a = coeffAFrag;
    float b = cross2D(relativePos2DFrag, quadDistortionFrag) + cross2D(edgeLeftFrag, edgeBottomFrag);
    float c = cross2D(relativePos2DFrag, edgeLeftFrag);

    // Determine 'u' and 'v' using a lot of fancy checks.
    float u = 0.0;
    float v = 0.0;
    
    // Parallelograms and top/bottom-collapsed triangles (a == 0)
    if (interpolateLinearlyFrag > 0) {
        u = (abs(b) < 1e-5)
            ? 0.5             // Utterly degenerate point; safely sample center
            : (-c * (1.0/b)); // Linear root
        v = calcV(u);
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

        u = q / a;
        v = calcV(u);

        // If (u, v) does not land inside the quad (with a tiny epsilon for edge floating point inaccuracy), use the other root.
        if (!(u >= -0.001 && u <= 1.001 && v >= -0.001 && v <= 1.001)) {
            // Fancy-shmancy way to calculate the other root for 'u' by using formulas by a
            // 16th century gentleman named "Vieta".
            u = c / q;
            v = calcV(u);
        }
    }

    return clamp(vec2(u, v), 0.0, 1.0);
}

void main() {
    if (meshFrag > 0.0) {
        ivec2 fragInt = ivec2(gl_FragCoord.xy) >> 1;
        if (((fragInt.x + fragInt.y) & 1) == 0) discard;
    }

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
