﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public enum Number
    {
        ERROR = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }
    public Number currentPlayer;
    
    private ScoreHandler _scoreHandler;

    public int lives = 5;
    private int currentLives;
    private bool canComeback;

    private void Start()
    {
        _scoreHandler = FindObjectOfType<ScoreHandler>();
        currentLives = lives;
    }

    private void OnTriggerStay(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball.canScore)
        {
            currentLives--;
            if (currentLives < 0)
                _scoreHandler.KillPlayer((int)currentPlayer);
            ball.canScore = false;
        }
    }

    public bool ResetLives()
    {
        if (canComeback)
        {
            currentLives = lives;
            canComeback = false;
            return true;
        }
        return false;
    }
}