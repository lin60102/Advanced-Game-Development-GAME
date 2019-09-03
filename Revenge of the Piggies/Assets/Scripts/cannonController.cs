using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonController : MonoBehaviour
{
    public GameObject piggyplayer; // reference to player object
    public GameObject mousepos;
    public float strenth = 100;
    float alpha;
    Vector3 mousePosition, mousePosInWorld,direction;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        direction = mousePosInWorld - transform.position;
        alpha = Mathf.Acos(Vector3.Dot(Vector3.right, direction.normalized))*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, alpha));

        if (Input.GetMouseButtonUp(0))
        {
            piggyplayer.transform.parent = null;
            piggyplayer.GetComponent<Rigidbody2D>().gravityScale = 1;
            piggyplayer.GetComponent<Rigidbody2D>().AddForce(direction * strenth);
        }
        /*****
        mousepos.transform.position = mousePosInWorld;
        this.transform.LookAt(mousepos.transform);

       
         */ //lookat method
    }
}
