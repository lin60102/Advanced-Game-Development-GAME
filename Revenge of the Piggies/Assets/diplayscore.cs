using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class diplayscore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<TextMeshProUGUI>().text = "Score: "+ ScoreManeger.instance.Score;
    }
}
