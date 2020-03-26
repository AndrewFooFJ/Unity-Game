using System.Collections;
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

    //public Animator loadingScreen;

    [Header("Crate Scriptable Objects")]
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
        SwitchCrateTypes();
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

    //open level selection page 1
    public void OpenLevelSelectPg1()
    {
        OpenClose(levelSelectPage[0], levelSelectPage[1]);
    }

    //open level selection page 2
    public void OpenLevelSelectPg2()
    {
        OpenClose(levelSelectPage[1], levelSelectPage[0]);
    }

    //open crate selection page 1 from level select screen
    public void OpenCrateSelection()
    {
        OpenClose(crateSelectionScreens[0], levelSelectScreen);
    }

    //open the store page
    public void OpenStorePage()
    {
        OpenClose(storePage, mainMenu);
    }

    //opens level selection
    public void OpenLevelSelectMenu()
    {
        OpenClose(levelSelectScreen, mainMenu);
        OpenClose(levelSelectPage[0], levelSelectPage[1]);
    }

    //open page 1 of the crate selection page
    public void OpenPage1()
    {
        OpenClose(crateSelectionScreens[0], crateSelectionScreens[1]);
        arrayInt = 0;
    }

    //open page 2 of crate selection page from page 1
    public void OpenPage2()
    {
        OpenClose(crateSelectionScreens[1], crateSelectionScreens[0]);
        arrayInt = 1;
    }

    //open page 2 of crate selection page from page 3
    public void OpenPage23()
    {
        OpenClose(crateSelectionScreens[1], crateSelectionScreens[2]);
        arrayInt = 1;
    }

    //open page 3 of crate selection page from page 2
    public void OpenPage3()
    {
        OpenClose(crateSelectionScreens[2], crateSelectionScreens[1]);
        arrayInt = 2;
    }

    void OpenClose(GameObject screenToOpen, GameObject screenToClose)
    {
        screenToOpen.SetActive(true);
        screenToClose.SetActive(false);
    }

    //open the first page of crate selection from the crate selection button
    public void CloseLevelSelection()
    {
        for (int p = 0; p <= 1; p++)
        {
            levelSelectPage[p].SetActive(false);
        }

        for (int p = 0; p <= 2; p++)
        {
            crateSelectionScreens[p].SetActive(false);
        }

        crateSelectionScreens[0].SetActive(true);
    }

    //open the first page of level selection from the level selection button
    public void CloseCrateSelection()
    {
        for (int p = 0; p <= 2; p++)
        {
            crateSelectionScreens[p].SetActive(false);
        }

        for (int p = 0; p <= 1; p++)
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
        for (int p = 0; p <= 2; p++)
        {
            crateSelectionScreens[p].SetActive(false);
        }

        for (int p = 0; p <= 0; p++)
        {
            levelSelectPage[p].SetActive(false);
        }

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
