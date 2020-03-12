using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeHinge : MonoBehaviour
{
    HingeJoint2D ropeHinge;
    public Rigidbody2D balloonRb;

    // Update is called once per frame
    void Update()
    {
        ropeHinge.connectedBody = balloonRb;
    }
}
