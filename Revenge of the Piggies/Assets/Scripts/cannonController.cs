using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonController : MonoBehaviour
{
    public Rigidbody2D piggyplayerRB; // reference to player object
    public GameObject mousepos;
    public Camera mainCamera;
    const float STRENTH = 100;
    const int ANGLEMAX = 80;
    const int ANGLEMIN = 10;
    float alpha;
    Vector3 mousePosition, mousePosInWorld,direction;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -mainCamera.transform.position.z);
        mousePosInWorld = mainCamera.ScreenToWorldPoint(mousePosition);
        direction = mousePosInWorld - transform.position;
        alpha = Mathf.Acos(Vector3.Dot(Vector3.right, direction.normalized))*Mathf.Rad2Deg;
        if (alpha <= ANGLEMAX && alpha > ANGLEMIN && direction.y>0)
        {
         transform.rotation = Quaternion.Euler(new Vector3(0, 0, alpha));
        }            
        /*****
        mousepos.transform.position = mousePosInWorld;
        this.transform.LookAt(mousepos.transform);
        */ //lookat method
    }
    void FixedUpdate()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            piggyplayerRB.transform.parent = null;
            piggyplayerRB.gravityScale = 1;
            piggyplayerRB.AddForce(direction * STRENTH * piggyplayerRB.mass);
        }

    }
}
