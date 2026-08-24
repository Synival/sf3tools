#version 330 core

uniform sampler2D textureAtlas;
uniform sampler2D textureOverlay1;
uniform sampler2D textureOverlay2;

in vec4 colorFrag;
in vec3 glowFrag;
in vec4 lightColorFrag;
in float meshFrag;

in vec2 texCoordAtlasFrag;
in vec2 texCoordOverlay1Frag;
in vec2 texCoordOverlay2Frag;

out vec4 FragColor;

void main() {
    if (meshFrag > 0 && (int(gl_FragCoord.x / 2) + int(gl_FragCoord.y / 2)) % 2 == 0)
        discard;

    vec4 surfaceTex = (texture(textureAtlas, texCoordAtlasFrag) + lightColorFrag);
    surfaceTex = surfaceTex * colorFrag + vec4(glowFrag, 0.0);

    vec4 overlayTex =
        texture(textureOverlay1, texCoordOverlay1Frag) +
        texture(textureOverlay2, texCoordOverlay2Frag);

    vec4 compositeColor = vec4(mix(surfaceTex.rgb, overlayTex.rgb, overlayTex.a), surfaceTex.a + overlayTex.a * (1.0 - surfaceTex.a));
    if (compositeColor.a < 0.001)
        discard;

    FragColor = compositeColor;
}
