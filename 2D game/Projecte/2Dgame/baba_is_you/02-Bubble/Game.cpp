#include <GL/glew.h>
#include <GL/glut.h>
#include "Game.h"


void Game::init()
{
	bPlay = true;
	glClearColor(0.082, 0.094, 0.121, 1.0f);
	engine = createIrrKlangDevice();
	scene = new MenuScene();
	scene->init(0);
}

bool Game::update(int deltaTime)
{
	scene->update(deltaTime);
	
	return bPlay;
}

void Game::render()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	scene->render();
}

void Game::keyPressed(int key)
{
	keys[key] = true;
}

void Game::exit() {
	bPlay = false;
}

void Game::keyReleased(int key)
{
	keys[key] = false;
}

void Game::specialKeyPressed(int key)
{
	specialKeys[key] = true;
}

void Game::specialKeyReleased(int key)
{
	specialKeys[key] = false;
}

void Game::mouseMove(int x, int y)
{
}

void Game::mousePress(int button)
{
}

void Game::mouseRelease(int button)
{
}

bool Game::getKey(int key) const
{
	return keys[key];
}

bool Game::getSpecialKey(int key) const
{
	return specialKeys[key];
}

bool Game::movementKeyPressed() {
	return 
		moveUpPressed() ||
		moveDownPressed() ||
		moveRightPressed() ||
		moveLeftPressed();
}


bool Game::resetKeyPressed() {
	return (Game::instance().getKey(82) || Game::instance().getKey(114));
}

bool Game::moveUpPressed() {
	return 
		getSpecialKey(GLUT_KEY_UP) ||
		getKey(87) ||
		getKey(119);
}

bool Game::moveDownPressed() {
	return
		getSpecialKey(GLUT_KEY_DOWN) ||
		getKey(83) ||
		getKey(115);
}

bool Game::moveRightPressed() {
	return
		getSpecialKey(GLUT_KEY_RIGHT) ||
		getKey(68) ||
		getKey(100);
}
bool Game::moveLeftPressed() {
	return
		getSpecialKey(GLUT_KEY_LEFT) ||
		getKey(65) ||
		getKey(97);
}


void Game::loopMusic(char* fileName) {
	//engine->removeSoundSource(fileName);
	if (!engine->isCurrentlyPlaying(fileName)) {
		stopMusic();
		music = engine->play2D(fileName, true, false, true);
		music->setVolume(0.5f);
		if (fileName == "music/Never_Back_Down_looped.wav") music->setVolume(0.2f);
	}
		
}

void Game::stopMusic() {
	if (music != NULL) {
		music->stop();
	}
}


void Game::playSound(char* fileName) {
	ISound* sound = engine->play2D(fileName, false, false, true);
	sound->setVolume(0.2f);
	sound->setPlaybackSpeed();
}

//sceneId = (1,7) 
void Game::changeScene(int sceneId) {

	delete scene;

	if (sceneId <= 5) {
		scene = new MapScene();
		scene->init(sceneId);
	}
	else if (sceneId == 6) {
		scene = new InstructionsScene();
		scene->init();
	}
	else if (sceneId == 7) {
		scene = new creditsScene();
		scene->init(); 
	}
}

void Game::changeSceneToMenu(int option) {
	delete scene;
	scene = new MenuScene();
	scene->init(option);
}

void Game::runConsole() {
	FILE* fp;
	AllocConsole();
	freopen_s(&fp, "CONIN$", "r", stdin);
	freopen_s(&fp, "CONOUT$", "w", stdout);
	freopen_s(&fp, "CONOUT$", "w", stderr);
}





