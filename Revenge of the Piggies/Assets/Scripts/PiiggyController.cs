using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiiggyController : MonoBehaviour
{
    Vector3 pos;
    Quaternion rota;
    Transform cannon;
    const float WAITTIME=3;
    // Start is called before the first frame update
    void Start()
    {
        
        cannon = transform.parent;
        pos = transform.position;
        rota = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        ScoreManeger.instance.Score++;
       // StartCoroutine("Resetpiggywait");
        Invoke("ResetPiggy", 3);
        //Destroy(gameObject, 3);
    }
    void ResetPiggy()
    {
        transform.position = pos;
        transform.rotation = rota;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        transform.parent = transform;
    }
    IEnumerator Resetpiggywait()
    {
        yield return new WaitForSeconds(WAITTIME);
        transform.position = pos;
        transform.rotation = rota;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        transform.parent = transform;

    }
}
