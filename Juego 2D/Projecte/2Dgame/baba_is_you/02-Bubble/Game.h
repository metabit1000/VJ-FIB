#ifndef _GAME_INCLUDE
#define _GAME_INCLUDE



#include "MapScene.h"
#include "MenuScene.h"
#include "CreditsScene.h"
#include "InstructionsScene.h"

#include <irrKlang.h>
#include <ik_ISoundEngine.h>
using namespace irrklang;

#define SCREEN_WIDTH 1280
#define SCREEN_HEIGHT 720


// Game is a singleton (a class with a single instance) that represents our whole application


class Game
{

public:
	Game() {}
	
	
	static Game &instance()
	{
		static Game G;
	
		return G;
	}
	
	void init();
	bool update(int deltaTime);
	void render();
	
	// Input callback methods
	void keyPressed(int key);
	void keyReleased(int key);
	void specialKeyPressed(int key);
	void specialKeyReleased(int key);
	void mouseMove(int x, int y);
	void mousePress(int button);
	void mouseRelease(int button);
	void exit();
	
	bool getKey(int key) const;
	bool getSpecialKey(int key) const;

	bool movementKeyPressed();
	bool resetKeyPressed();
	bool moveUpPressed();
	bool moveDownPressed();
	bool moveRightPressed();
	bool moveLeftPressed();

	void changeScene(int sceneId);
	void changeSceneToMenu(int option);
	void runConsole();

	void loopMusic(char* fileName);
	void stopMusic();
	void playSound(char* fileName);

private:
	bool bPlay;                       // Continue to play game?
	BaseScene* scene;                      // Scene to render
	bool keys[256], specialKeys[256]; // Store key states so that 
	                                  // we can have access at any time

	irrklang::ISoundEngine* engine; //para la musica del juego
	ISound* music;

};


#endif // _GAME_INCLUDE


