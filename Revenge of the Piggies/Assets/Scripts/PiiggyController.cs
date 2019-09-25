using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiiggyController : MonoBehaviour
{
    Vector3 pos, cannonpos;
    Quaternion rota,cannonrot;
    Transform cannon;
    public ScoreManeger scoremanager;
    public LevelManeger lvmag;
    public GameObject tntexplosion;
    const float WAITTIME=3;
    // Start is called before the first frame update
    void Start()
    {
        
        cannon = transform.parent;
        cannonpos = cannon.position;
        cannonrot = cannon.rotation;
        pos = transform.position;
        rota = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "bird")
        {
            Destroy(collision.gameObject);
            scoremanager.updateScore(10);    
        }
        if(collision.gameObject.tag == "block")
        {
            scoremanager.updateScore(1);
        }
        if (collision.gameObject.tag == "tnt")
        {
            GameObject explosion = Instantiate(tntexplosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(explosion, 0.5f);
            scoremanager.updateScore(1);
        }



        ///ScoreManeger.instance.Score++;
        //StartCoroutine("Resetpiggywait");
        Invoke("ResetPiggy", 3f);
        
        
    }
    void ResetPiggy()
    {
        cannon.position = cannonpos;
        cannon.rotation = cannonrot;
        transform.position = pos;
        transform.rotation = rota;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        transform.parent = cannon;
        //vmag.Updatelv(1);
    }/*
    IEnumerator Resetpiggywait()
    {
        yield return new WaitForSeconds(WAITTIME);
        transform.position = pos;
        transform.rotation = rota;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        transform.parent = cannon;
        lvmag.Updatelv(1);

    }*/
}
