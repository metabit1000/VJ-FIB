#version 330

uniform vec2 scalar;
in vec2 position;

void main()
{
	gl_Position = vec4(position*scalar, 0.0, 1.0);
}
