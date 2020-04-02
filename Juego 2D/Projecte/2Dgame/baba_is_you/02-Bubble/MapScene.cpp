#include <iostream>
#include <cmath>
#include <glm/gtc/matrix_transform.hpp>
#include "MapScene.h"
#include "Game.h"
#include <GL/glew.h>
#include <GL/glut.h>
#include <fstream>
#include <sstream>
#include <vector>


#define SCREEN_X 32
#define SCREEN_Y 16

#define INIT_PLAYER_X_TILES 4
#define INIT_PLAYER_Y_TILES 25


MapScene::MapScene()
{
	map = NULL;
	winState = false;
	loseState = false;
	enteredLoseState = false;
	cameraSpeed = 2.f;
	turn = 0.f;
}


MapScene::~MapScene()
{
	if(map != NULL)
		delete map;
}


void MapScene::init(int level)
{
	BaseScene::init();

	//Game::instance().runConsole();

	//load and init map 
	initMap("levels/level"+ to_string(level) +".txt");
	this->currentLevel = level;
	constantMovement = (currentLevel >= 4);
	movementDirection = ivec2(1, 0);

	//load background
	spritesheet.loadFromFile("images/black.png", TEXTURE_PIXEL_FORMAT_RGBA);
	background = Sprite::createSprite(map->getMapTotalSize(), glm::vec2(1.f, 1.f), &spritesheet, &texProgram);
	background->setPosition(map->getOrigin());

	spritesheetinst.loadFromFile("images/press_r.png", TEXTURE_PIXEL_FORMAT_RGBA);
	instructions = Sprite::createSprite(vec2(300.0, 20.0), glm::vec2(1.f, 1.f), &spritesheetinst, &texProgram);

	//init camera/screen camera elements
	camera = map->getOrigin() + map->getMapTotalSize() / 2;
	projection = glm::ortho(camera.x - float(SCREEN_WIDTH)/2, camera.x + float(SCREEN_WIDTH)/2, camera.y + float(SCREEN_HEIGHT)/2, camera.y - float(SCREEN_HEIGHT)/2);
	instructions->setPosition(camera + vec2(SCREEN_WIDTH / 2.f - SCREEN_WIDTH / 3.5f, SCREEN_HEIGHT / 2.f - SCREEN_HEIGHT / 8.f));
	if (currentLevel >= 4) updateCamera();

	//play background music

	if (currentLevel < 4) 	Game::instance().loopMusic("music/baba_is_you_ost.wav");
	else Game::instance().loopMusic("music/Never_Back_Down_looped.wav");
}


void MapScene::update(int deltaTime)
{
	currentTime += deltaTime;
	currentTurnTime += deltaTime;
	if (currentTime >= INT64_MAX || currentTurnTime < 0) currentTurnTime = TURN_TIME + 1;

	// movement input
	if (Game::instance().moveUpPressed()) movementDirection = ivec2(0, -1);
	else if (Game::instance().moveDownPressed()) movementDirection = ivec2(0, 1);
	else if (Game::instance().moveRightPressed()) movementDirection = ivec2(1, 0);
	else if (Game::instance().moveLeftPressed()) movementDirection = ivec2(-1, 0);

	//update object animations
	for (int i = 0; i < objects.size(); i++)
		objects[i]->update(deltaTime);


	if (winState) {
		Game::instance().playSound("music/Win.mp3");
		Sleep(500); //para dar tiempo a que suene el audio

		if (currentLevel == 5) Game::instance().changeScene(7);		//credits
		else Game::instance().changeScene(this->currentLevel + 1);	//next level
		
		//changing scene causes this scene instance to be deleted, we must return
		return;	
	}
	else if (loseState && !enteredLoseState) {
		enteredLoseState = true;

		//defeat sound and stop music
		Game::instance().playSound("music/Defeat.mp3"); 
		Game::instance().stopMusic();
	}
	else if (((Game::instance().movementKeyPressed() && currentTurnTime >= float(TURN_TIME)) || (currentTurnTime >= 2*float(TURN_TIME) && constantMovement)) && !loseState) {
		updateMapLogic();
		Game::instance().playSound("music/Baba_move.mp3");

		turn++; currentTurnTime = 0;
	}

	//update camera position
	if (currentLevel >= 4) updateCamera(deltaTime);

	//go to menu
	if (Game::instance().getKey(GLUT_KEY_ESC) && currentTurnTime >= float(TURN_TIME)) {
		Game::instance().changeSceneToMenu(this->currentLevel-1);
	}

	//reset level
	if (Game::instance().resetKeyPressed() && currentTurnTime >= float(TURN_TIME)) {
		Game::instance().changeScene(this->currentLevel); 
	}
}


void MapScene::render()
{
	glm::mat4 modelview;

	texProgram.use();
	texProgram.setUniformMatrix4f("projection", projection);
	texProgram.setUniform4f("color", 1.0f, 1.0f, 1.0f, 1.0f);
	modelview = glm::mat4(1.0f);
	texProgram.setUniformMatrix4f("modelview", modelview);
	texProgram.setUniform2f("texCoordDispl", 0.f, 0.f);

	background->render();
	if (loseState) instructions->render();
	map->render();
}


void MapScene::updateMapLogic() {
	//refresh objects (hasMoved = false)
	for (int i = 0; i < objects.size(); i++)
		objects[i]->refresh();

	//apply movement to objects
	for (int i = 0; i < objects.size(); i++) {
		Object* obj = objects[i];
		if (obj->isDestroyer() && turn % 2 || !obj->isDestroyer())
			obj->updateTurn(movementDirection);
	}


	//apply destruction by DESTROYER object
	for (int i = 0; i < objects.size(); i++) {
		if (objects[i]->isDestroyer()) {
			map->cleanBox(objects[i]->getMapPosition());
			map->cleanBox(objects[i]->getMapPosition() + ivec2(-1,0));
		}
	}

	//remove all properties (except words)
	for (int i = 0; i < objects.size(); i++) {
		Object* obj = objects[i];
		if (!obj->isWord()) obj->cleanProperties();
	}

	//check rules and apply new properties
	map->applyAllRules();


	//check win/defeat intersections and update win/lose state
	int controllableObjects = 0;
	for (int i = 0; i < objects.size(); i++) {

		if (objects[i]->hasProperty(IS_YOU)) {

			controllableObjects++;
			int objectState = objects[i]->checkState();

			if (objectState == 1) {
				winState = true;
			}
			else if (objectState == -1) {
				delete objects[i];
				objects[i] = NULL;
				controllableObjects--;
			}
		}	
	}
	if (controllableObjects == 0) loseState = true;


	//remove deleted objects from vector
	vector<Object*>::iterator i = objects.begin();
	while (i != objects.end()) {
		if (*i == NULL) i = objects.erase(i);
		else i++;
	}
}



void MapScene::updateCamera(int deltaTime) {

	float sumPositions_x = 0;
	int controllableObjects = 0;
	for (int i = 0; i < objects.size(); i++) {
		if (objects[i]->hasProperty(IS_YOU)) {
			controllableObjects++;
			sumPositions_x += objects[i]->getGlobalPosition().x;
		}
	}

	if (!controllableObjects) return;

	float objective = sumPositions_x / float(controllableObjects);
	camera.x += (objective - this->camera.x)*cameraSpeed*(deltaTime/1000.f);
	projection = glm::ortho(camera.x - float(SCREEN_WIDTH) / 2, camera.x + float(SCREEN_WIDTH) / 2, camera.y + float(SCREEN_HEIGHT) / 2, camera.y - float(SCREEN_HEIGHT) / 2);

	instructions->setPosition(camera + vec2(SCREEN_WIDTH / 2.f - SCREEN_WIDTH / 3.5f, SCREEN_HEIGHT / 2.f - SCREEN_HEIGHT / 8.f));
}


void MapScene::updateCamera() {

	float sumPositions_x = 0;
	int controllableObjects = 0;
	for (int i = 0; i < objects.size(); i++) {
		if (objects[i]->hasProperty(IS_YOU)) {
			controllableObjects++;
			sumPositions_x += objects[i]->getGlobalPosition().x;
		}
	}

	if (!controllableObjects) return;

	camera.x = sumPositions_x / float(controllableObjects);
	projection = glm::ortho(camera.x - float(SCREEN_WIDTH) / 2, camera.x + float(SCREEN_WIDTH) / 2, camera.y + float(SCREEN_HEIGHT) / 2, camera.y - float(SCREEN_HEIGHT) / 2);

	instructions->setPosition(camera + vec2(SCREEN_WIDTH / 2.f - SCREEN_WIDTH / 3.5f, SCREEN_HEIGHT / 2.f - SCREEN_HEIGHT / 8.f));
}

bool MapScene::initMap(const string& levelFile) {
	//obtengo datos del level

	ifstream fin;
	string line, tilesheetFile;
	stringstream sstream;
	int tile;
	ivec2 mapSize;
	ivec2 tileSize;

	fin.open(levelFile.c_str());
	if (!fin.is_open())
		return false;
	getline(fin, line);
	if (line.compare(0, 7, "TILEMAP") != 0)
		return false;
	getline(fin, line);
	sstream.str(line);
	sstream >> mapSize.x >> mapSize.y;
	getline(fin, line);
	sstream.str(line);
	sstream >> tileSize.x >> tileSize.y;
	map = new Map(glm::ivec2(SCREEN_X, SCREEN_Y), mapSize, tileSize);

	for (int y = 0; y < mapSize.y; y++)
	{
		for (int x = 0; x < mapSize.x; x++)
		{
			fin >> tile;
			if (tile == 1) {
				Object* object = new Object(map, texProgram, ITEM, BABA);
				object->setPos(x, y);
				objects.push_back(object);
			}
			else if (tile == 2) {
				Object* object = new Object(map, texProgram, ITEM, ROCK);
				object->setPos(x, y);
				objects.push_back(object);
			}
			else if (tile == 3) {
				Object* object = new Object(map, texProgram, ITEM, WALL);
				object->setPos(x, y);
				objects.push_back(object);
			}
			else if (tile == 4) {
				Object* object = new Object(map, texProgram, ITEM, FLAG);
				object->setPos(x, y);
				objects.push_back(object);
			}
			else if (tile == 5) {
				Object* object = new Object(map, texProgram, NOUN, BABA);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 6) {
				Object* object = new Object(map, texProgram, OPERATOR, IS);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 7) {
				Object* object = new Object(map, texProgram, PROPERTY, YOU);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 8) {
				Object* object = new Object(map, texProgram, NOUN, FLAG);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 9) {
				Object* object = new Object(map, texProgram, PROPERTY, WIN);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 10) {
				Object* object = new Object(map, texProgram, NOUN, WALL);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 11) {
				Object* object = new Object(map, texProgram, PROPERTY, STOP);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 12) {
				Object* object = new Object(map, texProgram, NOUN, ROCK);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 13) {
				Object* object = new Object(map, texProgram, PROPERTY, PUSH);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 14) {
				Object* object = new Object(map, texProgram, ITEM, SKULL);
				object->setPos(x, y);
				objects.push_back(object);
			}
			else if (tile == 15) {
				Object* object = new Object(map, texProgram, NOUN, SKULL);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 16) {
				Object* object = new Object(map, texProgram, PROPERTY, DEFEAT);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}
			else if (tile == 17) {
				Object* object = new Object(map, texProgram, OPERATOR, AND);
				object->setPos(x, y);
				object->addProperty(IS_PUSH);
				objects.push_back(object);
			}

			else if (tile == 21) {
				Object* object = new Object(map, texProgram, ITEM, DESTROYER);
				object->setPos(x, y);
				objects.push_back(object);
			}
			
		}
#ifndef _WIN32
		fin.get(tile);
#endif
	}
	fin.close();

	map->applyAllRules();
	return true;
}