using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class displaylv : Observer
{
    public override void OnNotfiy(object o, NotificationType n)
    {
        if (n == NotificationType.LevelUpdated)
        {
            GetComponent<TextMeshProUGUI>().text = "LV: " + o;
        }

    }

}
