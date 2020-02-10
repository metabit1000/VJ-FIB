#ifndef _CIRCLE_INCLUDE
#define _CIRCLE_INCLUDE

#include "ShaderProgram.h"


class Circle {

public:
	static Quad *createCircle(float x, float y, float width, float height, ShaderProgram &program);

	Circle(float x, float y, float width, float height, ShaderProgram &program);

	void render() const;
	void free();

private:
	GLuint vao;
	GLuint vbo;
	GLint posLocation;
};

#endif // _CIRCLE_INCLUDE