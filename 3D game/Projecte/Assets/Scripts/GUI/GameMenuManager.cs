using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;

    bool isPaused = false;
    bool winOrLoseMenu = false;
    GameObject player;


    public bool IsPaused() => isPaused;

    public void SetIsPaused(bool value) => isPaused = value;


    public void ViewLoseMenu()
    {
        winOrLoseMenu = true;
        loseMenu.SetActive(true);

        int score = GetComponent<GameManager>().GetScore();
        loseMenu.transform.Find("text_score").GetComponent<TextMeshProUGUI>().text = "SCORE: " + score.ToString();
    }

    public void ViewWinMenu()
    {
        if (player != null) player.GetComponent<Health>().SetGodMode(true);

        winOrLoseMenu = true;
        winMenu.SetActive(true);

        int score = winMenu.GetComponentInParent<GameManager>().GetScore();
        winMenu.transform.Find("text_score").GetComponent<TextMeshProUGUI>().text = "SCORE: " + score.ToString();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        if (player != null) player.GetComponent<PlayerController>().enabled = true;
        isPaused = false;
    }


    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        if (player != null) player.GetComponent<PlayerController>().enabled = false;
        isPaused = true;
    }


    void Start() 
    {
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        player = GameObject.Find("PlayerShip"); 
    }


    void Update()
    {
        if (winOrLoseMenu) return;

        if (Input.GetKeyDown("escape"))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

}
