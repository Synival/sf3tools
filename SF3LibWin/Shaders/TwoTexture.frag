#version 330 core

uniform sampler2D texture0;
uniform sampler2D texture1;

in vec4 colorFrag;
in vec2 texCoord0Frag;
in vec2 texCoord1Frag;

out vec4 FragColor;

void main() {
    vec4 tex1Color = texture(texture1, texCoord1Frag);
    FragColor = 
        texture(texture0, texCoord0Frag) * colorFrag * (1.0 - tex1Color.a) +
        vec4(tex1Color.rgb * tex1Color.a, tex1Color.a);
}
