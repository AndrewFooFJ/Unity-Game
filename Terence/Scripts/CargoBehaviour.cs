using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CargoBehaviour : MonoBehaviour {
    
    public List<BalloonBehaviour> attachedObjects;
    public AudioClip collisionSound;

    [Header("Damage")]
    public int damageThreshold = 1;
    public float velocityToDamageRatio = 2f; // Threshold at which we will take damage from a collision.
    BasicUnit unit; // Component for handling health.

    [Header("Physics")]
    public float baseDirection = -90f;
    public float landingLeeway = 25f;

    new AudioSource audio;
    new Rigidbody2D rigidbody;

    Vector2 lastCollisionVelocity; // For calculating impulse.

    // Start is called before the first frame update
    void Start() {
        audio = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        unit = GetComponent<BasicUnit>();
        unit.onDeath += OnDeath;
    }

    void FixedUpdate() {
        // If a force is registered, calculate the impulse.
        if(lastCollisionVelocity.sqrMagnitude > 0) {
            Vector2 impulse = lastCollisionVelocity - rigidbody.velocity;
            lastCollisionVelocity = Vector2.zero;
            int damage = (int)(impulse.magnitude / velocityToDamageRatio);
            if(damage > damageThreshold && unit.isAlive) {
                unit.Damage(damage);
                GameManager.instance.SetHealthBar(unit.health);
                Debug.Log(string.Format("{0} took {1} damage.",gameObject.name,damage),this);
            }
        }
    }

    // Draws the angles of the surfaces that we are allowed to hit from.
    void OnDrawGizmosSelected() {
        float rad = baseDirection * Mathf.Deg2Rad;
        Gizmos.color = Color.red;

        float radLower = (baseDirection + landingLeeway) * Mathf.Deg2Rad,
              radHigher = (baseDirection - landingLeeway) * Mathf.Deg2Rad;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(radLower), Mathf.Sin(radLower), 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(radHigher), Mathf.Sin(radHigher), 0));
    }

    void Reset() {
        attachedObjects = new List<BalloonBehaviour>(FindObjectsOfType<BalloonBehaviour>());
    }

    void OnCollisionEnter2D(Collision2D collision) {
        
        lastCollisionVelocity = collision.relativeVelocity;

        audio.PlayOneShot(
            collisionSound, 
            Mathf.Min(2f,collision.relativeVelocity.sqrMagnitude / 4)
        );
    }

    void OnDeath(GameObject instigator) {
        // Nulls the cargo to detach ourselves.
        foreach(BalloonBehaviour attached in attachedObjects) {
            attached.cargo = null;
        }
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
