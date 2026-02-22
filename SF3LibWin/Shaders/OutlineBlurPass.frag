#version 330 core

uniform sampler2D texture0;
uniform vec2 texelSize;
uniform vec2 blurDirectionVector;

in vec2 texCoord0Frag;

out vec4 FragColor;

const float weight[5] = float[5](
    0.2270270,
    0.1945946,
    0.1216216,
    0.0540541,
    0.0162162
);

void main() {
    vec4 result = texture(texture0, texCoord0Frag) * weight[0];

    for(int i = 1; i < 5; ++i) {
        result += texture(texture0, texCoord0Frag + texelSize * i * blurDirectionVector) * weight[i];
        result += texture(texture0, texCoord0Frag - texelSize * i * blurDirectionVector) * weight[i];
    }

    FragColor = result;
}
