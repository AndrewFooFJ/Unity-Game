using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleMovement : MonoBehaviour
{
    public int placementNodes;

    Rigidbody2D rb;
    public float speed;

    bool hasMoved = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchDirection();
        MoveRect();
    }

    void SwitchDirection()
    {
        switch (placementNodes)
        {
            case 0:
                rb.velocity = new Vector2(speed, 0); //move right
                break;

            case 1:
                rb.velocity = new Vector2(0, -speed); //move right
                break;

            case 2:
                rb.velocity = new Vector2(-speed, 0); //move right
                break;

            case 3:
                rb.velocity = new Vector2(0, speed); //move right
                break;
        }
    }

    void MoveRect()
    {
        if (hasMoved == false && placementNodes == 0)
        {
            rb.velocity = new Vector2(speed, 0); //move right
        }

        if (placementNodes >= 3)
        {
            placementNodes = 0;
        }    }
}
