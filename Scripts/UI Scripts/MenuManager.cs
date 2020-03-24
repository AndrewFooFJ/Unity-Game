﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Screen Variables")]
    public GameObject mainMenu;
    public GameObject creditPage;
    public GameObject levelSelectScreen;
    public GameObject[] levelSelectPage;
    public GameObject clearPlayerprefsScreen;
    public GameObject storePage;
    public GameObject[] crateSelectionScreens;

    public Animator loadingScreen;

    public CrateScriptableObject[] crateTypes;
    public Text[] crateName;
    public Text[] reason;
    public Image[] crateImg;
    int arrayInt;

    [Header("Text Variables")]
    public Text seedCountText;
    public Text liveCountText;

    private void Start()
    {

    }

    private void Update()
    {
        seedCountText.text = ": " + InGamePurchases.inGameCurrency;
        liveCountText.text = ": " + LevelManager.liveCount;

        SwitchCrateTypes();
    }

    void SwitchCrateTypes()
    {
        switch (arrayInt)
        {
            case 0:
                crateName[0].text = crateTypes[0].nameOfCrate;
                reason[0].text = crateTypes[0].unlockCritrea;
                crateImg[0].sprite = crateTypes[0].crateSprite;
                break;

            case 1:
                crateName[1].text = crateTypes[1].nameOfCrate;
                reason[1].text = crateTypes[1].unlockCritrea;
                crateImg[1].sprite = crateTypes[1].crateSprite;
                break;

            case 2:
                crateName[2].text = crateTypes[2].nameOfCrate;
                reason[2].text = crateTypes[2].unlockCritrea;
                crateImg[2].sprite = crateTypes[2].crateSprite;
                break;
        }
    }

    public void OpenCrateSelection()
    {
        OpenClose(crateSelectionScreens[0], levelSelectScreen);
    }

    public void OpenStorePage()
    {
        OpenClose(storePage, mainMenu);
    }

    //opens level selection
    public void OpenLevelSelectMenu()
    {
        OpenClose(levelSelectScreen, mainMenu);
    }

    public void OpenPage1()
    {
        OpenClose(crateSelectionScreens[0], crateSelectionScreens[1]);
    }

    public void OpenPage2()
    {
        OpenClose(crateSelectionScreens[1], crateSelectionScreens[0]);
    }

    public void OpenPage23()
    {
        OpenClose(crateSelectionScreens[1], crateSelectionScreens[2]);
    }

    public void OpenPage3()
    {
        OpenClose(crateSelectionScreens[2], crateSelectionScreens[1]);
    }

    void OpenClose(GameObject screenToOpen, GameObject screenToClose)
    {
        screenToOpen.SetActive(true);
        screenToClose.SetActive(false);
    }

    public void CloseLevelSelection()
    {
        for (int p = 0; p <= 0; p++)
        {
            levelSelectPage[p].SetActive(false);
        }

        for (int p = 0; p <= 2; p++)
        {
            crateSelectionScreens[p].SetActive(false);
        }

        crateSelectionScreens[0].SetActive(true);
    }

    public void CloseCrateSelection()
    {
        for (int p = 0; p <= 2; p++)
        {
            crateSelectionScreens[p].SetActive(false);
        }

        for (int p = 0; p <= 0; p++)
        {
            levelSelectPage[p].SetActive(false);
        }

        levelSelectPage[0].SetActive(true);
    }

    public void CloseStorePage()
    {
        OpenClose(mainMenu, storePage);
    }

    //closes level selection
    public void CloseLevelSelect()
    {
        OpenClose(mainMenu, levelSelectScreen);
    }

    public void OpenCreditPage()
    {
        OpenClose(creditPage, mainMenu);
    }

    public void CloseCreditPage()
    {
        OpenClose(mainMenu, creditPage);

    }

    //load level with selected scene name
    public void LoadSelectedLevel(string selectedScene)
    {
        //StartCoroutine(LoadingGame(loadingScreen, selectedScene));
        SceneManager.LoadScene(selectedScene); //load selected scene name
        Time.timeScale = 1f;
    }

    public void NoClear()
    {
        clearPlayerprefsScreen.SetActive(false);
    }

    public void YesClear()
    {
        PlayerPrefs.DeleteAll(); //clear all in game data

        SceneManager.LoadScene("Main Menu");
    }


    public void ClearPlayerprefs()
    {
        clearPlayerprefsScreen.SetActive(true);
    }

    //quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadingGame(Animator loadingScreenAnim, string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName); //load scene in background

        loadingScreenAnim.SetTrigger("StartLoading"); //play the starting of loading screen

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        loadingScreenAnim.SetTrigger("EndLoading"); //play the ending of the loading screen
    }
}
