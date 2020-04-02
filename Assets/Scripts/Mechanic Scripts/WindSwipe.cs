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

    // From Terence:
    // Don't think they are used. Please review and delete if not needed.
    // ------------------------------------
    //[Header("Swipe Variables")]
    //public bool swipe = false;
    //public GameObject newTrail;

    [Header("Boolean Variables")]
    //public bool canPush;
    public bool isPaused = false;
    public bool losesGame = false;
    bool hasSwipe = false;

    PlayerForces thePlayer;
    TrailRenderer trailRenderer; // Store the TrailRenderer here so we don't have to keep retrieving it using GetComponent().
    #endregion

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerForces>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    private void Update()
    {
        if (LevelManager.runGame == true)
        {
            //ClearWind(); //clear wind vertex on trail
            HandlePlayerInput(); //wind controls
        }
    }

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

    void UpdateTrailRenderer(TouchPhase phase) 
    {
        if(phase == TouchPhase.Began) 
        {
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            transform.position = new Vector3(touchPosition.x, touchPosition.y, 0);
            trailRenderer.Clear();
        }
        else if(phase == TouchPhase.Moved)
        {
            trailRenderer.emitting = true;
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            transform.position = new Vector3(touchPosition.x, touchPosition.y, 0);        
        }
        else if(phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
        {
            trailRenderer.emitting = false;
        }
    }

    // Renamed WindControls() to HandlePlayerInput() so it is more descriptive.
    void HandlePlayerInput()
    {
        
        // If we have lost the game, don't process player input anymore.
        if(LevelManager.gameIsLost) return;

        if(Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            UpdateTrailRenderer(t.phase);
        }
        else 
        {
            // If we are using mouse, map the mouse button events to TouchPhase events.
            if(Input.GetMouseButtonDown(0))
            {
                UpdateTrailRenderer(TouchPhase.Began);
            }
            else if(Input.GetMouseButton(0))
            {
                UpdateTrailRenderer(TouchPhase.Moved);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                UpdateTrailRenderer(TouchPhase.Ended);
            }
        }
    }
        #endregion
 }

