using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    Rigidbody2D balloonRb;
    HingeJoint2D balloonHinge;
    public Rigidbody2D ropeRb;

    public float speed;
    AudioSource balloonPopSound;
    public GameObject balloonExplosion;
    public bool hasPop = false;
    public float timeBefDissapear;

    LevelManager theLM;
    WindSwipe playerSwipe;
    CameraController theCam;

    // Start is called before the first frame update
    void Start()
    {
        balloonRb = GetComponent<Rigidbody2D>(); //get rigidbody2D component
        balloonHinge = GetComponent<HingeJoint2D>();

        playerSwipe = FindObjectOfType<WindSwipe>();
        theCam = FindObjectOfType<CameraController>();
        theLM = FindObjectOfType<LevelManager>();
        balloonPopSound = GameObject.Find("Balloon Pop Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.runGame)
        {
            balloonRb.velocity = new Vector2(balloonRb.velocity.x, speed); //fly up for now

            balloonHinge.connectedBody = ropeRb;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spike")
        {
            Pop();
            Debug.Log("Spike is hit");
        }
    }

    public void ResetBalloon()
    {
        theLM.BalloonAnimation();
        balloonHinge.connectedBody = ropeRb;
    }

    void Pop()
    {
        hasPop = true;
        LevelManager.PlayAudioSource(balloonPopSound); //play pop sound
        LevelManager.runGame = false;
        Destroy(Instantiate(balloonExplosion, transform.position, Quaternion.identity), timeBefDissapear); //spawn balloon explosion and despawn after 0.5f
        theLM.LoseGame(); //lose game when player pops balloon
        gameObject.SetActive(false);
        Debug.Log("It has poped");
    }
}
