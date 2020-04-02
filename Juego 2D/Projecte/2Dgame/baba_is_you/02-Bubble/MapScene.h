#ifndef _SCENE_INCLUDE
#define _SCENE_INCLUDE

#include "BaseScene.h"

#include <glm/glm.hpp>
#include "ShaderProgram.h"
#include "Map.h"
#include <vector>
#include "Object.h"



// Scene contains all the entities of our game.
// It is responsible for updating and render them.


class MapScene: public BaseScene
{

public:
	MapScene();
	~MapScene();

	void init(int level);
	void update(int deltaTime);
	void render();

private:
	bool initMap(const string& levelFile);
	void updateCamera();
	void updateCamera(int deltaTime);
	void updateMapLogic();

private:
	Map *map;
	vector<Object*> objects;

	bool winState, loseState;
	bool enteredLoseState; 

	int turn;
	int currentLevel; 
	vec2 camera;
	float cameraSpeed;

	Sprite* instructions;
	Texture spritesheetinst;

	ivec2 movementDirection;
	bool constantMovement;
};


#endif // _SCENE_INCLUDE

