using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class GameMenuManager : MonoBehaviour {

    public static GameMenuManager instance;

    [Header("Scenes")]
    public string levelSelectScene;
    public string nextLevelScene;

    [Header("Menus")]
    public GameObject overlay;
    public GameObject basicMenu, completeMenu, tutorialMenu;
    Transform stars;

    [Header("Resources")]
    public Sprite starEmpty;
    public Sprite starFull;
    public AudioClip starJingle, levelCompleteJingle;

    public const float STARS_ITERATE_DELAY = 0.4f;

    public IEnumerator SetStars(int count) {
        if(completeMenu.activeSelf) {
            stars = completeMenu.transform.Find("Stars");
            for(int i = 0; i < count; i++) {
                // Starts playing the particle system on the star.
                Transform s = stars.GetChild(i);
                ParticleSystem p = s.GetComponent<ParticleSystem>();
                p.Play(true);

                yield return new WaitForSecondsRealtime(p.main.duration - 0.1f);

                // Get the star to light up.
                Image img = s.GetComponent<Image>();
                img.sprite = starFull;

                // Play the audio for star jingle.
                GameManager.instance.audio.pitch = 0.7f + 0.18f * i;
                GameManager.instance.audio.PlayOneShot(starJingle);

                yield return new WaitForSecondsRealtime(STARS_ITERATE_DELAY);
            }
            GameManager.instance.audio.pitch = 1f;
        }
    }

    // For hooking to buttons.
    public void Open(string menuType) {
        overlay.SetActive(false); // Disables the overlay.
        gameObject.SetActive(true);
        StartCoroutine(Open(0f, menuType));
    }

    public void Open(string menuType, float displayDelay, string description = "", int stars = 3) {
        overlay.SetActive(false); // Disables the overlay.
        gameObject.SetActive(true);
        StartCoroutine(Open(displayDelay, menuType, description, stars));
    }

    public IEnumerator Open(float displayDelay, string menuType, string description = "", int stars = 3) {
        if(displayDelay > 0) yield return new WaitForSeconds(displayDelay);

        overlay.SetActive(true);
        OnEnable();

        switch(menuType) {
            case "Paused":
            case "Game Over":
                // Prevent these screens from firing if we have won.
                if(GameManager.instance.levelState == GameManager.LevelState.victory)
                    yield break;

                Text basicMenuTitle = basicMenu.transform.Find("Title").GetComponent<Text>();
                basicMenuTitle.text = menuType;

                // Opens the basic menu.
                if(basicMenu) basicMenu.SetActive(true);
                if(completeMenu) completeMenu.SetActive(false);
                break;
            case "Level Complete":
                if(basicMenu) basicMenu.SetActive(false);
                if(completeMenu) completeMenu.SetActive(true);

                if(stars > 0) StartCoroutine(SetStars(stars));
                if(levelCompleteJingle) GameManager.instance.audio.PlayOneShot(levelCompleteJingle);
                break;
            case "Tutorial":

                Text tutorialTitle = tutorialMenu.transform.Find("Title").GetComponent<Text>();
                tutorialTitle.text = menuType;

                Text tutorialDescription = tutorialMenu.transform.Find("Description").GetComponent<Text>();
                tutorialDescription.text = description;
                break;
        }
    }

    public void Resume() {
        gameObject.SetActive(false);
    }

    public void Replay() {
        gameObject.SetActive(false);
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
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

    // Autofill these fields.
    void Reset() {
        overlay = transform.Find("Overlay").gameObject;
        if(overlay) {
            basicMenu = overlay.transform.Find("BasicMenu").gameObject;
            completeMenu = overlay.transform.Find("CompleteMenu").gameObject;
        }
    }
}
