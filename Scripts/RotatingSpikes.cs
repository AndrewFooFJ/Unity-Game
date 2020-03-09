using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpikes : MonoBehaviour
{
    public float rotatingSpeed;

    private void Start()
    {

    }

    private void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, rotatingSpeed));
    }
}
