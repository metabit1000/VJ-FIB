#ifndef _CIRCLE_INCLUDE
#define _CIRCLE_INCLUDE

#include "ShaderProgram.h"


class Circle {

public:
	static Circle *createCircle(float x, float y, float r, ShaderProgram &program);

	Circle(float x, float y, float r, ShaderProgram &program);

	void render() const;
	void free();

private:
	GLuint vao;
	GLuint vbo;
	GLint posLocation;
	float pi = acos(-1.0f); //calculamos el numero pi
	float resol = pi / 16;
};

#endif // _CIRCLE_INCLUDE