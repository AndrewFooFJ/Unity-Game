using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class GameMenuManager : MonoBehaviour {

    public static GameMenuManager instance;

    public string levelSelectScene;

    [Header("Menus")]
    public GameObject overlay;
    public GameObject basicMenu;
    public GameObject completeMenu;

    public void Open(string menuType, float displayDelay = 0f) {
        overlay.SetActive(false); // Disables the overlay.
        gameObject.SetActive(true);
        StartCoroutine(Open(displayDelay, menuType));
    }

    public IEnumerator Open(float displayDelay, string menuType) {
        if(displayDelay > 0) yield return new WaitForSeconds(displayDelay);

        overlay.SetActive(true);

        switch(menuType) {
            case "Paused":
            case "Game Over":
                Text basicMenuTitle = basicMenu.GetComponentInChildren<Text>();
                basicMenuTitle.text = menuType;

                // Opens the basic menu.
                if(basicMenu) basicMenu.SetActive(true);
                if(completeMenu) completeMenu.SetActive(false);
                break;
            case "Level Complete":
                if(basicMenu) basicMenu.SetActive(false);
                if(completeMenu) completeMenu.SetActive(true);
                break;
        }
    }

    public void Resume() {
        gameObject.SetActive(false);
    }

    public void Replay() {
        gameObject.SetActive(false);
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }

    public void ToLevelSelect() {
        if(levelSelectScene.Length <= 0) Debug.LogError("No scene assigned for Level Select.", this);
        gameObject.SetActive(false);
        SceneManager.LoadScene( levelSelectScene );
    }

    void OnEnable() {
        if(overlay && overlay.activeSelf) Time.timeScale = 0f;
    }

    void OnDisable() {
        Time.timeScale = 1f;
    }

    // Saves an instance to a static variable.
    // Gives a warning if there is more than 1 of this class.
    void Start() {
        if(instance) Debug.LogWarning("More than 1 GameMenuManager in the scene!", gameObject);
        else instance = this;

        gameObject.SetActive(false);
    }
}
