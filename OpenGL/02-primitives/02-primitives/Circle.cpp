#include <GL/glew.h>
#include <GL/gl.h>
#include "Quad.h"

Circle *Circle::createQuad(float x, float y, float width, float height, ShaderProgram &program)
{
	Circle *circle = new Circle(x, y, width, height, program);

	return circle;
}


Circle::Circle(float x, float y, float width, float height, ShaderProgram &program)
{
	float vertices[12] = {}; ///aqui van los vertices para crear un circulo

	glGenVertexArrays(1, &vao);
	glBindVertexArray(vao);
	glGenBuffers(1, &vbo);
	glBindBuffer(GL_ARRAY_BUFFER, vbo);
	glBufferData(GL_ARRAY_BUFFER, 12 * sizeof(float), vertices, GL_STATIC_DRAW);
	posLocation = program.bindVertexAttribute("position", 2);
}

void Circle::render() const
{
	glBindVertexArray(vao);
	glEnableVertexAttribArray(posLocation);
	glDrawArrays(GL_TRIANGLES, 0, 6);
}

void Circle::free()
{
	glDeleteBuffers(1, &vbo);
}