#version 330 core

uniform sampler2D textureAtlas;
uniform sampler2D textureTerrainType;

in vec3 colorFrag;
in vec3 glowFrag;
in float lightingFrag;

in vec2 texCoordAtlasFrag;
in vec2 texCoordTerrainTypeFrag;
in vec2 texCoordEventIDFrag;

out vec4 FragColor;

void main() {
    vec4 surfaceTex = (texture(textureAtlas, texCoordAtlasFrag) * vec4(colorFrag, 1.0)
      + vec4(glowFrag, 0.0)
      + vec4(vec3(1.0, 1.0, 0.5) * lightingFrag * 0.2, 0));
    vec4 terrainTypeTex = texture(textureTerrainType, texCoordTerrainTypeFrag);

    FragColor = vec4(mix(surfaceTex.rgb, terrainTypeTex.rgb, terrainTypeTex.a), surfaceTex.a + terrainTypeTex.a * (1.0 - surfaceTex.a));
}
