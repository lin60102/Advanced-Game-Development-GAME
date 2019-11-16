using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private RaycastHit lastRaycastHit;
    public Transform headset;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward * 100);
        Debug.DrawLine(transform.position, transform.position + transform.forward * 100, Color.red);
        if (Physics.Raycast(ray, out lastRaycastHit, 10) && OVRInput.GetDown(OVRInput.Button.One))
        {
            headset.position=lastRaycastHit.point;


        }
    }
}
