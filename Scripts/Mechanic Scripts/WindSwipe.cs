using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindSwipe : MonoBehaviour
{
    #region Variables
    [Header("Touch and Wind Variables")]
    //public Slider windPowerIndicator;
    public Vector3 touchPosition;
    public int windPower;

    [Header("Swipe Variables")]
    public bool swipe = false;
    public GameObject newTrail;

    [Header("Time Variables")]
    public float startingRechargeTime;
    public float rechargeTime;
    public float timeBefDespawn;
    public float startTimeBefDespawn;
    public float timeBefSpawning;
    public float timeBefChangeWind = 5f;

    [Header("Boolean Variables")]
    //public bool canPush;
    public bool isPaused = false;
    public bool losesGame = false;
    bool hasSwipe = false;

    PlayerForces thePlayer;
    #endregion

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerForces>();
    }

    private void Update()
    {
        if (LevelManager.runGame == true)
        {
            ClearWind(); //clear wind vertex on trail
            WindControls(); //wind controls
            Debug.Log("player can move");
        }
        else
        {
            Debug.Log("player cant move");
        }
    }

    #region Wind Create Functions
    //creates a ribbon like trail to symbolise the wind direction
    public void CreateWind()
    {
        timeBefDespawn = startTimeBefDespawn;
        GetComponent<TrailRenderer>().emitting = true;

        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        this.transform.position = new Vector3(touchPosition.x, touchPosition.y, 0);
    }

    public void ClearWind()
    {
        if (hasSwipe == true)
        {
            timeBefDespawn -= Time.deltaTime;
        }
        
        //if time has went to 0
        if (timeBefDespawn <= 0)
        {
            GetComponent<TrailRenderer>().Clear();
        }
    }
    #endregion

    #region Player Wind Controls Function
    void CalculateWindPower(int xDirection, int yDirection)
    {
        int x;
        int y;

        x = xDirection;
        y = yDirection;

        //if x is less than 0
        if (x > 0)
        {
            x = -x;
        }

        //if y is less than 0
        if (y > 0)
        {
            y = -y;
        }

        //decrease wind power with the calculations
        windPower -= (int)((thePlayer.forceMultiplyer *= thePlayer.direction.x) + (thePlayer.forceMultiplyer *= thePlayer.direction.y));
    }

    void WindControls()
    {
        //Debug.Log("it can do this 2");

        //only enable when there is 1 finger on screen
        //if (Input.touchCount == 1)
        //{
            //Debug.Log("it can do this");

            //if mouse button is held down or player has place finger on screen, also if canPush is true
            if ((Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)) && !LevelManager.gameIsLost)
            {
                hasSwipe = false;
                CreateWind(); //just creates a ribbon of the trail renderer
                Debug.Log("Player can swipe");
            }
            else if ((Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && !LevelManager.gameIsLost)
            {
                hasSwipe = true;
            } else
            {
                Debug.Log("player cant swipe");
            }
        //}
    }
        #endregion
 }

