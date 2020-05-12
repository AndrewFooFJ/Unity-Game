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
    [HideInInspector] public new AudioSource audio; // Public to allow other objects to play sounds.

    [System.Serializable]
    public struct UIElements {
        public GameObject HUD;
        public Text remainingTime, distanceToObjective;
        public Image objectivePointer;
        public Sprite objectivePointerNear;
        public Slider healthSlider;
        public Vector2 screenMargin;
        internal Sprite objectivePointerSprite;
    }

    [Header("UI & SFX")]
    public UIElements HUDElements;

    float remainingTime;
    
    public enum LevelState { preGame, inGame, victory, defeat }
    public LevelState levelState { get; protected set; } = LevelState.preGame;

    void Awake() {
        if(instance) Debug.LogWarning("More than 1 GameManager in the scene!", gameObject);
        else instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        
        remainingTime = completionTimes[2];
        camera = cameraBehaviour.GetComponent<Camera>();
        audio = cameraBehaviour.GetComponent<AudioSource>() ?? GetComponent<AudioSource>();

        HUDElements.objectivePointerSprite = HUDElements.objectivePointer.sprite;
        UpdateUI();

        levelState = LevelState.preGame;
        StartCoroutine(GameBeginCoroutine());
    }

    IEnumerator GameBeginCoroutine() {
        cameraBehaviour.isFollowing = false;
        yield return new WaitForSeconds(startDelay);
        cameraBehaviour.isFollowing = true;

        // Detect when we are close to the target, so that we can start the game.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float dist = 0.8f;
        while(cameraBehaviour.SqrDistanceToTarget() > dist) {
            yield return w;
        }
        
        // Set camera behaviour.
        cameraBehaviour.movementMode = GameCameraBehaviour.MovementMode.lerp;

        // Start the level.
        levelState = LevelState.inGame;
    }

    // Update is called once per frame
    void Update() {
        if(levelState == LevelState.inGame) {
            remainingTime = Mathf.Max(0,remainingTime - Time.deltaTime);
            if(remainingTime <= 0) {
                NotifyDefeat(1f);
            }
        }
        UpdateUI();
    }

    void UpdateUI() {
        HUDElements.remainingTime.text = "Time: " + Mathf.Ceil(remainingTime);

        // Don't run this if player is dead.
        if(player)
            HUDElements.distanceToObjective.text = Mathf.Ceil(Vector2.Distance(player.position, goal.position)).ToString();

        Vector3 screenPoint = camera.WorldToScreenPoint(goal.position);

        // If objective is in camera, make it smaller and change its shape.
        if(screenPoint.x < Screen.width && screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.y < Screen.height) {
            HUDElements.objectivePointer.sprite = HUDElements.objectivePointerNear;
            screenPoint = new Vector3(screenPoint.x, screenPoint.y + 90f, screenPoint.z);

            // Nulls any rotation when we are near.
            HUDElements.objectivePointer.transform.rotation = Quaternion.identity;
        } else {
            HUDElements.objectivePointer.sprite = HUDElements.objectivePointerSprite;

            // Rotate it to point to the objective.
            Vector2 diff = goal.position - transform.position;
            HUDElements.objectivePointer.transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg);
        }

        // Move the objective marker.
        HUDElements.objectivePointer.transform.position = new Vector3(
            Mathf.Max(
                0 + HUDElements.screenMargin.x,
                Mathf.Min(Screen.width - HUDElements.screenMargin.x,screenPoint.x)
            ),
            Mathf.Max(
                0 + HUDElements.screenMargin.y,
                Mathf.Min(Screen.height - HUDElements.screenMargin.y,screenPoint.y)
            ),
            0
        );
    }

    public void SetHealthBar(int value) {
        if(HUDElements.healthSlider) {
            HUDElements.healthSlider.value = value;

            GameObject fillArea = HUDElements.healthSlider.transform.Find("Fill Area").gameObject;
            if(value <= 0) {
                if(fillArea.activeSelf) fillArea.SetActive(false);
            } else {
                if(!fillArea.activeSelf) fillArea.SetActive(true);
            }
        }
    }

    public void NotifyDefeat(float delay = 4f) {
        
        if(levelState != LevelState.inGame) return;

        // Turns off the HUD and opens the game over screen.
        HUDElements.objectivePointer.gameObject.SetActive(false);
        HUDElements.HUD.SetActive(false);
        GameMenuManager.instance.Open("Game Over", delay);
        levelState = LevelState.defeat;

        // Pop the player's balloon.
        CargoBehaviour cargo = player.GetComponent<CargoBehaviour>();
        if(cargo) cargo.Pop();
    }

    public void NotifyVictory(float delay = 2f) {
        if(levelState != LevelState.inGame) return;

        levelState = LevelState.victory;
        HUDElements.objectivePointer.gameObject.SetActive(false);
        HUDElements.HUD.SetActive(false);
        GameMenuManager.instance.Open("Level Complete", delay);
    }
}
