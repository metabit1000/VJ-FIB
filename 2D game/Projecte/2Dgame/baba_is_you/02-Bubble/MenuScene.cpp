#include "MenuScene.h"


MenuScene::MenuScene() {}
MenuScene:: ~MenuScene() {}

void MenuScene::init(int level) {
	BaseScene::init();

	spritesheet.loadFromFile("images/menu.png", TEXTURE_PIXEL_FORMAT_RGBA);
	background = Sprite::createSprite(vec2(1280.0,720.0), glm::vec2(1.f, 1.f), &spritesheet, &texProgram);
	background->setPosition(vec2(0.0,0.0));

	projection = glm::ortho(0.f, float(SCREEN_WIDTH - 1), float(SCREEN_HEIGHT - 1), 0.f);
	
	loadSprites();
	
	this->option = level;
	
	//play background music
	Game::instance().loopMusic("music/baba_is_you_ost.wav");
}

void MenuScene::loadSprites() {
	
	for (int i = 0; i <= 4; ++i) {
		options[i].spritesheet.loadFromFile("images/level_image" + to_string(i+1) + ".png", TEXTURE_PIXEL_FORMAT_RGBA);
		options[i].sprite = Sprite::createSprite(glm::ivec2(194, 151), glm::vec2(1.f, 1.f), &options[i].spritesheet, &texProgram);
		options[i].sprite->setPosition(vec2(51.6+(i*245.6), 337));
	}
	
	options[5].spritesheet.loadFromFile("images/Instructions.png", TEXTURE_PIXEL_FORMAT_RGBA);
	options[5].sprite = Sprite::createSprite(glm::ivec2(270, 60), glm::vec2(1.f , 1.f), &options[5].spritesheet, &texProgram);
	options[5].sprite->setPosition(vec2(float(SCREEN_WIDTH - 525), float(SCREEN_HEIGHT - 60)));

	options[6].spritesheet.loadFromFile("images/Credits.png", TEXTURE_PIXEL_FORMAT_RGBA);
	options[6].sprite = Sprite::createSprite(glm::ivec2(150, 60), glm::vec2(1.f, 1.f), &options[6].spritesheet, &texProgram);
	options[6].sprite->setPosition(vec2(float(SCREEN_WIDTH - 200), float(SCREEN_HEIGHT - 60)));
}

void MenuScene::update(int deltaTime) {
	currentTime += deltaTime;
	currentTurnTime += deltaTime;

	if (currentTurnTime >= float(TURN_TIME)) {
		if (Game::instance().moveRightPressed()) {
			if (option < 6) ++option;
			currentTurnTime = 0;
		}

		else if (Game::instance().moveLeftPressed()) {
			if (option > 0) --option;
			currentTurnTime = 0;
		}

		else if (Game::instance().getKey(GLUT_KEY_ENTER)) {
			Game::instance().changeScene(option + 1);
			Game::instance().playSound("music/OptionMenu.mp3");
		}

		else if (Game::instance().getKey(GLUT_KEY_ESC)) {
			Game::instance().exit();
		}
		
	}
	

}

void MenuScene::render() {
	glm::mat4 modelview;

	texProgram.use();
	texProgram.setUniformMatrix4f("projection", projection);
	texProgram.setUniform4f("color", 1.0f, 1.0f, 1.0f, 1.0f);
	modelview = glm::mat4(1.0f);
	texProgram.setUniformMatrix4f("modelview", modelview);
	texProgram.setUniform2f("texCoordDispl", 0.f, 0.f);

	background->render();
	
	for (int i = 0; i < 7; i++) {
		if (option != i) {
			options[i].sprite->move(vec2(1.f, sin(currentTime/100.0f)));
		}
		else {
			options[i].sprite->resize(1.25f);
		}
	}
		
	
}