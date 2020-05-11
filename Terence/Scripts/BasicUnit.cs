using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour {

    // Defines the DamageFeedback struct for <damageFeedback>.
    [System.Serializable]
    public struct DamageFeedback {
        public bool enabled;
        public float duration;
        public Color32 colour, originalColour;
        public Renderer[] renderers;
    }

    [Header("Health")]
    public bool invulnerable = false;
    public int health = 100, maxHealth = 100;

    [Header("Feedback")]
    public DamageFeedback damageFeedback;
    public GameObject hitEffectPrefab;
    public float deathTime = 5f, deathFadeTime = 1.89f;

    public delegate void OnDeath(GameObject instigator = null);
    public OnDeath onDeath;

    public delegate void OnDamaged(int amount, GameObject instigator, Vector3 location);
    public OnDamaged onDamaged;

    // Only Actor is allowed to control isAlive's value. Children and subclasses cannot.
    public bool isAlive { get; protected set; } = true;

    protected virtual void Awake() {
    }

    // Fills up any variables that we need to fill.
    protected virtual void Reset() {
        FindRenderers();
        damageFeedback.enabled = true;
        damageFeedback.colour = new Color32(255,0,0,255);
        damageFeedback.originalColour = new Color32(255,255,255,255);
        damageFeedback.duration = 0.12f;
    }

    // Populates the list with Renderers.
    public void FindRenderers() {
        Renderer[] sr = GetComponents<Renderer>(), 
                    children = GetComponentsInChildren<Renderer>(true);
        List<Renderer> r = new List<Renderer>();
        r.AddRange(sr);
        r.AddRange(children);
        damageFeedback.renderers = r.ToArray();
    }

    // Deal damage to this Actor. Takes an optional MonoBehaviour object to allow one to specify who dealt
    // the damage. Returns the amount of damage dealt (if there are reductions).
    public virtual int Damage(int amount, GameObject instigator = null) {

        if (!invulnerable) {
            health = Mathf.Max(0, health - amount);
        }

        // Show damage feedback.
        if (damageFeedback.renderers.Length > 0 && damageFeedback.enabled) {
            StartCoroutine(DamageFlash(amount,instigator));
        }

        // Handle player dying.
        if (health <= 0) {
            if(isAlive) Death(instigator);
        } else if(health > maxHealth) health = maxHealth;

        return amount;
    }

    // Override the Damage() function above, not this one.
    public virtual int Damage(int amount, GameObject instigator, Vector3 location) {
        // If a hit effect is specified, play it.
        if(hitEffectPrefab) {
            Instantiate(hitEffectPrefab, location, transform.rotation);
        }

        return Damage(amount,instigator);
    }

    // Damage() calls this function when the Actor dies. You can also manually call this function to 
    // kill off the Actor.
    public virtual void Death(GameObject instigator = null) {
        isAlive = false;
        health = 0;
        Destroy(gameObject,deathTime);

        onDeath?.Invoke(instigator);

        if(deathFadeTime > 0) {
            float dft = Mathf.Min(deathFadeTime,deathTime); // Make sure <deathFadeTime> is capped at <deathTime>.
            StartCoroutine(DeathFade(damageFeedback.renderers,deathTime - dft,dft));
        }
    }

    // For handling the hit flash when a character receives damage.
    protected virtual IEnumerator DamageFlash(int amount, GameObject instigator = null) {
        foreach(Renderer sr in damageFeedback.renderers) {
            if(sr is SpriteRenderer) (sr as SpriteRenderer).color = damageFeedback.colour;
            else sr.material.color = damageFeedback.colour;
        }
        yield return new WaitForSeconds(damageFeedback.duration);
        foreach(Renderer sr in damageFeedback.renderers) {
            if(sr is SpriteRenderer) (sr as SpriteRenderer).color = damageFeedback.originalColour;
            else sr.material.color = damageFeedback.originalColour;
        }
    }

    protected virtual IEnumerator DeathFade(Renderer[] objects, float delay, float duration) {
        yield return new WaitForSeconds(delay); // Wait before destroying.

        // Fade this object away.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float dur = duration, opacity;
        while(dur > 0) {
            opacity = dur / deathFadeTime;
            foreach(Renderer sr in objects) {
                if(sr is SpriteRenderer) {
                    SpriteRenderer src = sr as SpriteRenderer;
                    src.color = new Color(src.color.r, src.color.g, src.color.b, opacity);
                } else {
                    sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, opacity);
                }
            }
            dur -= Time.deltaTime;
            yield return w;
        }
    }
}
