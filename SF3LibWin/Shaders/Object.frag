#version 330 core

uniform sampler2D textureAtlas;
uniform sampler2D textureTerrainTypes;
uniform sampler2D textureEventIDs;

in vec4 colorFrag;
in vec3 glowFrag;
in vec4 lightColorFrag;

in vec2 texCoordAtlasFrag;
in vec2 texCoordTerrainTypesFrag;
in vec2 texCoordEventIDsFrag;

out vec4 FragColor;

void main() {
    vec4 surfaceTex = (texture(textureAtlas, texCoordAtlasFrag) + lightColorFrag);
    surfaceTex = surfaceTex * colorFrag + vec4(glowFrag, 0.0);

    vec4 overlayTex =
        texture(textureTerrainTypes, texCoordTerrainTypesFrag) +
        texture(textureEventIDs,     texCoordEventIDsFrag);

    vec4 compositeColor = vec4(mix(surfaceTex.rgb, overlayTex.rgb, overlayTex.a), surfaceTex.a + overlayTex.a * (1.0 - surfaceTex.a));
    if (compositeColor.a < 0.001)
        discard;

    FragColor = compositeColor;
}
