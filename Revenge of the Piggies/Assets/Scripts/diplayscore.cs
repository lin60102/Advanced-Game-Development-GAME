using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class diplayscore : Observer
{
    public override void OnNotfiy(object o, NotificationType n)
    {
        if(n==NotificationType.ScoreUpdate)
        {
            GetComponent<TextMeshProUGUI>().text = "Score: " + o;
        }
       
    }

}
