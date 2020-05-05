using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [HideInInspector] public static GameManager instance;
    public GameCameraBehaviour cameraBehaviour;

    [Header("Level Settings")]
    public float startDelay = 1.5f;
    public float[] completionTimes = new float[3];
    public Transform player, goal;

    Camera camera;

    [System.Serializable]
    public struct UIElements {
        public Text remainingTime, distanceToObjective;
        public Image objectivePointer;
        public Sprite objectivePointerNear;
        public float screenMargin;
        internal Sprite objectivePointerSprite;
    }
    public UIElements HUDElements;

    float remainingTime;
    [HideInInspector] public bool levelStarted = true;

    // Start is called before the first frame update
    void Start() {
        if(instance) Debug.LogWarning("More than 1 GameManager in the scene!", gameObject);
        else instance = this;

        remainingTime = completionTimes[2];
        camera = cameraBehaviour.GetComponent<Camera>();

        HUDElements.objectivePointerSprite = HUDElements.objectivePointer.sprite;
        UpdateUI();

        StartCoroutine(GameBeginCoroutine());
    }

    IEnumerator GameBeginCoroutine() {
        cameraBehaviour.isFollowing = false;
        yield return new WaitForSeconds(startDelay);
        cameraBehaviour.isFollowing = true;

        // Wait until we are close to the target.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float dist = cameraBehaviour.moveSpeed / Time.fixedDeltaTime;
        print(dist);
        while(cameraBehaviour.SqrDistanceToTarget() > dist) {
            yield return w;
        }

        // Set camera behaviour.
        cameraBehaviour.movementMode = GameCameraBehaviour.MovementMode.lerp;
    }

    // Update is called once per frame
    void Update() {
        if(levelStarted) {
            //print(remainingTime);
            remainingTime -= Time.deltaTime;
            UpdateUI();
        }
    }

    void UpdateUI() {
        HUDElements.remainingTime.text = "Time: " + Mathf.Ceil(remainingTime);
        HUDElements.distanceToObjective.text = Mathf.Ceil(Vector2.Distance(player.position, goal.position)).ToString();

        Vector3 screenPoint = camera.WorldToScreenPoint(goal.position);

        // If objective is in camera, make it smaller and change its shape.
        if(screenPoint.x < Screen.width && screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.y < Screen.height) {
            HUDElements.objectivePointer.sprite = HUDElements.objectivePointerNear;
            screenPoint = new Vector3(screenPoint.x, screenPoint.y + 90f, screenPoint.z);
        } else {
            HUDElements.objectivePointer.sprite = HUDElements.objectivePointerSprite;
        }

        // Move the objective marker.
        HUDElements.objectivePointer.transform.position = new Vector3(
            Mathf.Max(
                0 + HUDElements.screenMargin,
                Mathf.Min(Screen.width - HUDElements.screenMargin,screenPoint.x)
            ),
            Mathf.Max(
                0 + HUDElements.screenMargin,
                Mathf.Min(Screen.height - HUDElements.screenMargin,screenPoint.y)
            ),
            0
        );
    }

    public void NotifyVictory() {
        HUDElements.objectivePointer.gameObject.SetActive(false);
        GameMenuManager.instance.Open("Level Complete", 3f);
        levelStarted = false;
    }
}
