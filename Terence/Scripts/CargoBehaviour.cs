using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CargoBehaviour : MonoBehaviour {
    
    public List<BalloonBehaviour> attachedObjects;
    public AudioClip collisionSound;

    new AudioSource audio;

    // Start is called before the first frame update
    void Start() {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void Reset() {
        attachedObjects = new List<BalloonBehaviour>(FindObjectsOfType<BalloonBehaviour>());
    }

    void OnCollisionEnter2D(Collision2D collision) {
        audio.PlayOneShot(
            collisionSound, 
            Mathf.Min(2f,collision.relativeVelocity.sqrMagnitude / 4)
        );
    }

    public void Pop(BalloonBehaviour target) {
        int index = attachedObjects.IndexOf(target);
        if(index > -1) Pop(index);
    }

    // Destroy an object attached to us.
    public void Pop(int index = -1) {
        
        // This prevents a Stack Overflow, since this method calls GameManager.NotifyGameOver(),
        // which calls this method again.
        if(attachedObjects.Count <= 0) return;

        // Pop the object.
        if(index > -1) {
            attachedObjects[index].Death();
            attachedObjects.RemoveAt(index);
        }  else {
            for(int i=0; i < attachedObjects.Count; i++) {
                attachedObjects[i].Death();
            }
            attachedObjects.Clear();
        }

        // If there are no more attached objects, then game is over.
        if(attachedObjects.Count <= 0) {
            GameManager.instance.NotifyDefeat();
        }
    }
}
