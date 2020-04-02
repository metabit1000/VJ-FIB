#include "creditsScene.h"
#include "Game.h"

creditsScene::creditsScene() {

}

creditsScene::~creditsScene() {

}

void creditsScene::init() {
	BaseScene::init();

	spritesheet.loadFromFile("images/credits_image.png", TEXTURE_PIXEL_FORMAT_RGBA);
	background = Sprite::createSprite(vec2(1280.0,720.0), glm::vec2(1.f, 1.f), &spritesheet, &texProgram);
	background->setPosition(vec2(0.0, 0.0));

	projection = glm::ortho(0.f, float(SCREEN_WIDTH - 1), float(SCREEN_HEIGHT - 1), 0.f);
}

void creditsScene::update(int deltaTime) {
	if (Game::instance().getKey(GLUT_KEY_ESC)) {
		Game::instance().changeSceneToMenu(6);
	}
}

void creditsScene::render() {
	glm::mat4 modelview;

	texProgram.use();
	texProgram.setUniformMatrix4f("projection", projection);
	texProgram.setUniform4f("color", 1.0f, 1.0f, 1.0f, 1.0f);
	modelview = glm::mat4(1.0f);
	texProgram.setUniformMatrix4f("modelview", modelview);
	texProgram.setUniform2f("texCoordDispl", 0.f, 0.f);

	background->render();
}
