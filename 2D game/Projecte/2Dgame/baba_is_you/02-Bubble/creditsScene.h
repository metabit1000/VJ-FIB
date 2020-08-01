#pragma once
#include "BaseScene.h"

class creditsScene : public BaseScene
{
	public:
		creditsScene();
		~creditsScene();
		
		void init();
		void update(int deltaTime);
		void render();
	private:
		//nada
};

