using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HUDPopper : MonoBehaviour {

    public Collider2D[] targets; // Valid targets for the collision.
    public int triggerCount = 1;

    [System.Serializable]
    public class Message {
        public string menuType = "Tutorial", description = "Sample HUD description.";
        public int stars = 0;
        public float popDelay = 0.3f;
    }
    public Message[] messages;

    void OnTriggerEnter2D(Collider2D other) {

        // Don't fire anymore if there is no more trigger count.
        if(triggerCount <= 0) return;

        if(targets.Contains(other)) {

            triggerCount--;

            // Pop all the messages.
            StartCoroutine(Display());
        }
    }

    IEnumerator Display() {
        for(int i=0;i<messages.Length;i++) {
            yield return new WaitForSeconds(messages[i].popDelay);
            GameMenuManager.instance.Open(
                messages[i].menuType, 0,
                messages[i].description, messages[i].stars
            );
        }

        Destroy(gameObject);
    }
}
