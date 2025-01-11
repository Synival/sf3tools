#version 330 core

uniform sampler2D textureAtlas;
uniform sampler2D textureTerrainTypes;
uniform sampler2D textureEventIDs;

in vec3 colorFrag;
in vec3 glowFrag;
in float lightingFrag;

in vec2 texCoordAtlasFrag;
in vec2 texCoordTerrainTypesFrag;
in vec2 texCoordEventIDsFrag;

out vec4 FragColor;

void main() {
    vec4 surfaceTex =
        (texture(textureAtlas, texCoordAtlasFrag) * vec4(colorFrag, 1.0) +
        vec4(glowFrag, 0.0) +
        vec4(vec3(1.0, 1.0, 0.5) * lightingFrag * 0.2, 0));

    vec4 overlayTex =
        texture(textureTerrainTypes, texCoordTerrainTypesFrag) +
        texture(textureEventIDs,     texCoordEventIDsFrag);

    FragColor = vec4(mix(surfaceTex.rgb, overlayTex.rgb, overlayTex.a), surfaceTex.a + overlayTex.a * (1.0 - surfaceTex.a));
    if (FragColor.a < 0.001)
        discard;
}
