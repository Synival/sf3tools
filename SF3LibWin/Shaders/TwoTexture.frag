#version 330 core

uniform sampler2D texture0;
uniform sampler2D texture1;

in vec4 colorFrag;
in vec2 texCoord0Frag;
in vec2 texCoord1Frag;

out vec4 FragColor;

void main() {
    vec4 tex0Color = texture(texture0, texCoord0Frag);
    vec4 tex1Color = texture(texture1, texCoord1Frag);

    vec4 compositeColor = tex0Color * colorFrag * (1.0 - tex1Color.a) + vec4(tex1Color.rgb * tex1Color.a, tex1Color.a);
    if (compositeColor.a < 0.001)
        discard;

    FragColor = compositeColor;
}
