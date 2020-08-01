using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int score = 0;
    float countdown = 1f;

    private void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            countdown = 1 + countdown;
            score--;
        }
    }

    public int GetScore() => score;


    public void AddScore(int points)
    {
        score += points;
    }


    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void LoadScene(int level)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(level);
    }


    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }


    public void CloseGame()
    {
        //ignored inside editor
        Application.Quit();
    }


    public void GodMode(bool value)
    {
        GameObject player = GameObject.Find("PlayerShip");

        if (player != null)
        {
            Health health = player.GetComponent<Health>();
            if (health != null) health.SetGodMode(value);
        }
    }

    public void TurnMusic(bool value)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        AudioSource source = camera.GetComponent<AudioSource>();
        if (source != null) {
            if (value) source.Play();
            else source.Stop();
        }
    }

}
