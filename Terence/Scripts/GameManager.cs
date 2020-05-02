using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [HideInInspector] public static GameManager instance;
    public GameCameraBehaviour cameraBehaviour;
    public float[] completionTimes = new float[3];
    public Transform player, goal;

    Camera camera;

    [System.Serializable]
    public struct UIElements {
        public Text remainingTime, distanceToObjective;
        public Image objectivePointer;
    }
    public UIElements HUDElements;

    float remainingTime;
    [HideInInspector] public bool levelStarted = true;

    // Start is called before the first frame update
    void Start() {
        instance = this;
        remainingTime = completionTimes[2];
        camera = cameraBehaviour.GetComponent<Camera>();
        UpdateUI();
    }

    // Update is called once per frame
    void Update() {
        if(levelStarted) {
            print(remainingTime);
            remainingTime -= Time.deltaTime;
            UpdateUI();
        }
    }

    void UpdateUI() {
        HUDElements.remainingTime.text = "Time: " + Mathf.Ceil(remainingTime);
        HUDElements.distanceToObjective.text = Mathf.Ceil(Vector2.Distance(player.position, goal.position)).ToString();

        Vector3 screenPoint = camera.WorldToScreenPoint(goal.position);
        HUDElements.objectivePointer.transform.position = new Vector3(
            Mathf.Min(Screen.width,screenPoint.x), 
            Mathf.Min(Screen.height,screenPoint.y),
            0
        );
    }
}
