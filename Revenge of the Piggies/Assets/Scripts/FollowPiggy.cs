using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPiggy : MonoBehaviour
{
    public Transform Piggy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 goal = new Vector3(Piggy.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, goal, 0.1f);
    }
}
