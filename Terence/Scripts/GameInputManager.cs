using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour {

    BalloonBehaviour[] controlledItems;
    BalloonBehaviour lastControlledItem;

    public Camera targetCamera;
    public float doubleClickThreshold = 0.15f;

    // Nested class for storing the last click on balloon.
    public class Click {
        public float startTime, duration = 0f;
        public bool isActive = true;
    }
    readonly List<Click> clicks = new List<Click>(2); // Record of clicks made.
    const int CLICK_LIST_LENGTH = 2;

    // Start is called before the first frame update
    void Start() {
        controlledItems = FindObjectsOfType<BalloonBehaviour>();
    }

    // Update is called once per frame
    void Update() {

        // Don't receive player input unless game is on.
        if(GameManager.instance.levelState != GameManager.LevelState.inGame) return;
        if(Time.timeScale == 0f) return; // Ignore all clicks when paused.

        // Get a target to control.
        BalloonBehaviour balloon = GetClosestTarget(targetCamera.ScreenToWorldPoint(Input.mousePosition));
        if(!balloon) return;

        // Process clicking and unclicking.
        if(Input.GetMouseButtonDown(0)) {
            // Register click, but also limit length of clicks.
            if(clicks.Count == CLICK_LIST_LENGTH) clicks.RemoveAt(0);
            clicks.Add(new Click { startTime = Time.time });

            // Perform different actions depending on whether we have a double click.
            if(HasDoubleClick() && lastControlledItem == balloon) {
                if(balloon.cargo) balloon.cargo.Pop(balloon);
                else balloon.Death();
            } else {
                balloon.Inflate(true);
                lastControlledItem = balloon;
            }
        } else if(Input.GetMouseButtonUp(0)) {
            // Process unclicking.
            if(clicks.Count > 0)
                clicks[clicks.Count - 1].isActive = false;

            // Orders the balloon to stop inflating.
            if(lastControlledItem)
                lastControlledItem.Inflate(false);
        }

        // Update the current click duration if any.
        if(clicks.Count > 0) {
            if(clicks[clicks.Count - 1].isActive)
                clicks[clicks.Count - 1].duration += Time.deltaTime;
        }
    }

    // Get the closest balloon from a position.
    public BalloonBehaviour GetClosestTarget(Vector2 position) {
        // Quick return values if we have 0 or 1 item.
        if(controlledItems.Length < 1) return null;
        else if(controlledItems.Length == 1) return controlledItems[0];

        // Start looping through all the targets.
        float minDist = float.MaxValue;
        int selectedItem = -1;
        for(int i = 0; i<controlledItems.Length; i++) {

            // If the balloon is no longer there, disregard.
            if(!controlledItems[i]) continue;

            float dist = Vector2.Distance(position,controlledItems[i].transform.position);
            if(dist < minDist) {
                minDist = dist;
                selectedItem = i;
            }
        }

        // Return the selected item.
        if(selectedItem > -1) return controlledItems[selectedItem];
        return null;
    }

    // Checks the clicks list to see if we have a double click.
    public bool HasDoubleClick() {
        int count = clicks.Count;
        if(count > 1) {
            if(clicks[count-1].startTime + clicks[count-1].duration - clicks[count-2].startTime <= doubleClickThreshold)
                return true;
        }
        return false;
    }
}
