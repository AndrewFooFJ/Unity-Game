using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraController : MonoBehaviour {

    public Transform[] targets;
    public bool isFollowing = true;
    public float smoothing = 1f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 destination = Vector3.zero;
        for(int i = 0; i < targets.Length; i++) destination += targets[i].position;
        destination /= targets.Length;

        // Move towards destination.
        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(destination.x, destination.y, transform.position.z),
            smoothing * Time.deltaTime
        );
    }
}
