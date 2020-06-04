using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScorePoints : MonoBehaviour
{
    public int points = 25;

    GameManager gameManager;
    Health health;

    public void AddScore()
    {
        gameManager.AddScore(points);
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        health = GetComponent<Health>();
        if (health != null) health.OnDeath += OnDeath;
    }

    void OnDisable()
    {
        if (health != null) health.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        gameManager.AddScore(points);
    }
}
