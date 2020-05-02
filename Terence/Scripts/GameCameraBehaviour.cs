using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraBehaviour : MonoBehaviour {

    public Transform[] targets;
    public Vector3 offset;
    public bool isFollowing = true;
    public float smoothing = 1f, moveSpeed = 20f;

    public enum MovementMode { lerp, constant }
    public MovementMode movementMode;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 destination = Vector3.zero;
        for(int i = 0; i < targets.Length; i++) destination += targets[i].position;
        destination /= targets.Length;

        // Move towards destination.
        switch(movementMode) {
            default: case MovementMode.lerp:
                transform.position = Vector3.Lerp(
                    transform.position, 
                    new Vector3(destination.x, destination.y, transform.position.z) + offset,
                    smoothing * Time.deltaTime
                );
                break;
            case MovementMode.constant:
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    new Vector3(destination.x, destination.y, transform.position.z) + offset,
                    moveSpeed * Time.deltaTime
                );
                break;
        }
    }
}
