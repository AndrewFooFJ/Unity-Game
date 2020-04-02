using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForces : MonoBehaviour
{
    #region Variables
    Rigidbody2D playerRb;

    WindSwipe theWind;
    Balloon theBalloon;

    //bool runGame = true;

    public float forceMultiplyer;

    [Header("Time Variables")]
    public float startTimeBefChangeWind;
    public float timeBefChangeWind;

    [Header("Direction Variables")]
    public Vector3 startPos, endPos, direction;
    #endregion

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerRb.gravityScale = 1;

        theBalloon = FindObjectOfType<Balloon>();
        theWind = FindObjectOfType<WindSwipe>();

        timeBefChangeWind = startTimeBefChangeWind; //set time to be starting time

        //StartCoroutine(RunGame());
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        //StopMoving();
        CalculateMouseDrag();
    }

    public void StopMoving()
    {
        playerRb.gravityScale = 0;

        //allow the crate to move the a direction for a few sec before switching back to its normal speed
       /* if (theWind.touchPosition.x > 0 ||
            theWind.touchPosition.x < 0 ||
            theWind.touchPosition.y > 0 ||
            theWind.touchPosition.y < 0)
        {
            timeBefChangeWind -= Time.deltaTime;
        } else if (timeBefChangeWind <= 0)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y);
            timeBefChangeWind = startTimeBefChangeWind;
        }*/
    }

    public void PlayerMovement()
    {
        //check if the balloon has pop, in order for the crate to act normally, rather than having to float in mid air
        if (!theBalloon.hasPop || LevelManager.runGame == true)
        {
            playerRb.velocity = new Vector2(forceMultiplyer * direction.x, forceMultiplyer * direction.y);

            //Debug.Log("Balloon is following player commands");
        }
        else if (theBalloon.hasPop || !LevelManager.runGame)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y);

            Debug.Log("Balloon is not following player commands");
        }
    }

    //this check the mouse is dragging in what direction
    public void CalculateMouseDrag()
    {
        //only enable when there is 1 finger on screen
        //if (Input.touchCount == 1)
        //{
        if (LevelManager.runGame)
        {

            // From Terence: Cleaned up the code by breaking the if condition into a few blocks.
            if(Input.touchCount > 0)
            {
                // Store the Touch so that you don't have to keep retrieving it whenever you want
                // to check it.
                Touch t = Input.GetTouch(0);

                // When touch / mouse is started.
                if (t.phase == TouchPhase.Began)
                {
                    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                // When touch / mouse is released.
                else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                {
                    endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

            } else {

                // If there is no touch input, listen for mouse clicks.
                if(Input.GetMouseButtonDown(0)) {
                    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }

            direction = endPos - startPos; //find the dist between end and start positions
        }        
    }

    /*IEnumerator RunGame()
    {
        while (runGame == true)
        {
            PlayerMovement();
            StopMoving();
            CalculateMouseDrag();

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }*/
}
