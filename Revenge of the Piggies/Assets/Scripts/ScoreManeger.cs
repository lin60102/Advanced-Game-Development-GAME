using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManeger : Subject
{
    public  int Score = 0;
    public Observer displayScore;

    public static ScoreManeger instance = new ScoreManeger();

    public void Start()
    {
        registerObserver(displayScore);
    }
    public void updateScore(int point)
    {
        Score += point;
        Notify(Score, NotificationType.ScoreUpdate);
    }
}
