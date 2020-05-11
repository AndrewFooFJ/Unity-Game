using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCameraBehaviour : MonoBehaviour {

    [Header("Target")]
    public bool isFollowing = true;
    public Transform[] targets;
    public Vector3 offset;
    
    
    public enum MovementMode { lerp, constant, custom }

    [Header("Movement")]
    public MovementMode movementMode;
    public float smoothing = 1f, moveSpeed = 20f;
    public AnimationCurve customMovement;

    [HideInInspector] public Vector3 destination { get; protected set; }

    [Header("UI")]
    public Canvas canvas;
    public float fadeDuration = 3f;
    Image fader;

    // Start is called before the first frame update
    void Start() {
        Fade(Color.black, "In", fadeDuration);
    }

    void Fade(Color fadeColor, string direction = "In", float duration = 2f) {

        if(!fader) {
            // If there is no canvas, don't bother drawing.
            if(!canvas) {
                Debug.LogWarning("No canvas to draw the fade effect on.", this);
                return;
            }

            // Create a new GameObject and attach it to the canvas.
            GameObject go = new GameObject("Camera Fader");
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.SetParent(canvas.transform);
            rect.SetAsFirstSibling();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(1,1);
            rect.anchoredPosition = rect.sizeDelta = Vector3.zero;
            fader = go.AddComponent<Image>();
            fader.fillAmount = 1f;
        }
        
        fader.color = fadeColor; // Assign fader color.

        // Use cross fade alpha to fade.
        switch(direction) {
            case "In": default:
                fader.CrossFadeAlpha(0f,duration,true);
                break;
            case "Out":
                fader.CrossFadeAlpha(1f,duration,true);
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        if(!isFollowing) return;

        destination = Vector3.zero;
        int total = 0;
        for(int i = 0; i < targets.Length; i++) {
            if(!targets[i]) continue;
            destination += targets[i].position;
            total++;
        }
        if(total <= 0) return; // If there are no valid targets, abort.
        destination = (destination / total) + offset;

        // Move towards destination.
        switch(movementMode) {
            default: case MovementMode.lerp:

                transform.position = Vector3.Lerp(
                    transform.position, 
                    destination,
                    smoothing * Time.deltaTime
                );
                break;

            case MovementMode.constant:
            case MovementMode.custom:

                // Use the graph for movement if we are using custom movement mode.
                float speed = moveSpeed * Time.deltaTime;
                if(movementMode == MovementMode.custom)
                    speed = customMovement.Evaluate(DistanceToTarget()) * Time.deltaTime;
                
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    destination,
                    moveSpeed * Time.deltaTime
                );
                break;
        }
    }

    public float SqrDistanceToTarget() {
        Vector2 v = transform.position - destination;
        return v.sqrMagnitude;
    }

    public float DistanceToTarget() {
        Vector2 v = transform.position - destination;
        return v.magnitude;
    }
}
