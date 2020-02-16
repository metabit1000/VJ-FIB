#include <iostream>
#include "Scene.h"
#include "Circle.h"


Scene::Scene()
{
	for(int i = 0; i < 4; i++)
		//quads[i] = NULL;
		cercles[i] = NULL;
}

Scene::~Scene()
{
	for(int i = 0; i < 4; i++)
		/*if(quads[i] != NULL)
			delete quads[i];*/
		if(cercles[i] != NULL)
			delete cercles[i];
}


void Scene::init()
{
	initShaders();
	for (int i = 0; i < 4; i++)
		//quads[i] = Quad::createQuad(-0.75f + (i % 2), -0.75f + (i / 2), 0.5f, 0.5f, program);
		/* 0: x = -0.75 y = -0.75
		   1: x = 0.25  y = -0.25
		   2: x = -0.75 y = 0.25
		   3: x = 0.25  y = 0.75
		*/
		cercles[i] = Circle::createCircle(-0.50f + (i % 2), -0.50f + (i / 2), 0.30f ,program);
}

void Scene::update(int deltaTime)
{
}

void Scene::render()
{
	program.use();
	for (int i = 0; i < 4; i++)
		//quads[i]->render();
		cercles[i]->render();
}

void Scene::initShaders()
{
	Shader vShader, fShader;

	vShader.initFromFile(VERTEX_SHADER, "shaders/simple.vert");
	if(!vShader.isCompiled())
	{
		cout << "Vertex Shader Error" << endl;
		cout << "" << vShader.log() << endl << endl;
	}
	fShader.initFromFile(FRAGMENT_SHADER, "shaders/simple.frag");
	if(!fShader.isCompiled())
	{
		cout << "Fragment Shader Error" << endl;
		cout << "" << fShader.log() << endl << endl;
	}
	program.init();
	program.addShader(vShader);
	program.addShader(fShader);
	program.link();
	if(!program.isLinked())
	{
		cout << "Shader Linking Error" << endl;
		cout << "" << program.log() << endl << endl;
	}
	program.bindFragmentOutput("outColor");
}

