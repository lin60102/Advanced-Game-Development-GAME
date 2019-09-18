using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManeger : Subject
{
    public int lv = 1;
    public Observer displaylv;

    public static LevelManeger lvinstance = new LevelManeger();

    public void Start()
    {
        registerObserver(displaylv);
    }
    public void Updatelv(int lvpoint)
    {
        lv += lvpoint;
        Notify(lv, NotificationType.LevelUpdated);
    }
}