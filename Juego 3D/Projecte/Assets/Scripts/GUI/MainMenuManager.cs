using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject instructions;
    public GameObject credits;

    private GameObject currentView;


    public void ViewMainMenu()
    {
        currentView.SetActive(false);
        currentView = mainMenu;

        mainMenu.SetActive(true);
    }

    public void ViewLevelMenu()
    {
        currentView.SetActive(false);
        currentView = levelMenu;
        currentView.SetActive(true);
    }

    public void ViewCredits()
    {
        currentView.SetActive(false);
        currentView = credits;
        currentView.SetActive(true);
    }

    public void ViewInstructions()
    {
        currentView.SetActive(false);
        currentView = instructions;
        currentView.SetActive(true);
    }


    private void Start()
    {
        currentView = mainMenu;
        currentView.SetActive(true);

        levelMenu.SetActive(false);
        credits.SetActive(false);
        instructions.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            ViewMainMenu();
        }
    }

}
