#include "InstructionsScene.h"
#include "Game.h"

#define TURN_TIME 120

InstructionsScene::InstructionsScene() {
	this->window = 0;
	currentTime = 0.0f;
	currentTurnTime = 0.0f;
}

InstructionsScene:: ~InstructionsScene(){}

void InstructionsScene::init() {
	BaseScene::init();

	spritesheet.loadFromFile("images/instructions1.png", TEXTURE_PIXEL_FORMAT_RGBA);
	background = Sprite::createSprite(vec2(1280.0, 720.0), glm::vec2(1.f, 1.f), &spritesheet, &texProgram);
	background->setPosition(vec2(0.0, 0.0));
	
	projection = glm::ortho(0.f, float(SCREEN_WIDTH - 1), float(SCREEN_HEIGHT - 1), 0.f);
}

void InstructionsScene::changeBackgroud() {
	spritesheet.loadFromFile("images/instructions"+to_string(window+1)+".png", TEXTURE_PIXEL_FORMAT_RGBA);
	background = Sprite::createSprite(vec2(1280.0, 720.0), glm::vec2(1.f, 1.f), &spritesheet, &texProgram);
	background->setPosition(vec2(0.0, 0.0));
}

void InstructionsScene::update(int deltaTime) {
	currentTime += deltaTime;
	
	currentTurnTime += deltaTime;
	if (currentTurnTime >= float(TURN_TIME)) {
		if (Game::instance().getSpecialKey(GLUT_KEY_RIGHT)) {
			if (window < 2) ++window;
			changeBackgroud();
			currentTurnTime = 0;
		}

		if (Game::instance().getSpecialKey(GLUT_KEY_LEFT)) {
			if (window > 0) --window;
			changeBackgroud();
			currentTurnTime = 0;
		}
	}

	if (Game::instance().getKey(GLUT_KEY_ESC)) {
		Game::instance().changeSceneToMenu(5); //cambio a MenuScene
	}
}

void InstructionsScene::render() {
	glm::mat4 modelview;

	texProgram.use();
	texProgram.setUniformMatrix4f("projection", projection);
	texProgram.setUniform4f("color", 1.0f, 1.0f, 1.0f, 1.0f);
	modelview = glm::mat4(1.0f);
	texProgram.setUniformMatrix4f("modelview", modelview);
	texProgram.setUniform2f("texCoordDispl", 0.f, 0.f);

	//render del sprite que nos interesa
	background->render();
}
