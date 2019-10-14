using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 38, 164.5f);
        if (transform.eulerAngles.x > 60 || transform.eulerAngles.x < -45)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
            Debug.Log("test X");
        }
        if (transform.eulerAngles.z > 60 || transform.eulerAngles.z < -45)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            Debug.Log("test Z");
        }
        if (transform.eulerAngles.y > 60 || transform.eulerAngles.y < -45)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            Debug.Log("test Z");
        }
    }
}
