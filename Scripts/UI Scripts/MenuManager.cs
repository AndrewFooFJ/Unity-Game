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
        //close level select and Credit Page and open main menu by default
        /*CloseLevelSelect(); 
        CloseCreditPage();*/
        //OpenCrateSelection();
        //OpenStorePage();
        //clearPlayerprefsScreen.SetActive(false);
    }

    private void Update()
    {
        seedCountText.text = "Paw Coins: " + InGamePurchases.inGameCurrency;
        liveCountText.text = "Lives: " + LevelManager.liveCount;

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

    public void StartGame()
    {
        //load level 1 for now
        SceneManager.LoadScene("GameScene");
    }

    public void OpenCrateSelection()
    {
        mainMenu.SetActive(false);
        crateSelectionScreens[0].SetActive(true);
    }

    public void OpenStorePage()
    {
        mainMenu.SetActive(false);
        storePage.SetActive(true);
    }

    //opens level selection
    public void OpenLevelSelect()
    {
        mainMenu.SetActive(false);
        levelSelectScreen.SetActive(true);
    }

    public void OpenPage1()
    {
        PageSelectForCrates(crateSelectionScreens[0], crateSelectionScreens[1]);
    }

    public void OpenPage2()
    {
        PageSelectForCrates(crateSelectionScreens[1], crateSelectionScreens[0]);
    }

    public void OpenPage23()
    {
        PageSelectForCrates(crateSelectionScreens[1], crateSelectionScreens[2]);
    }

    public void OpenPage3()
    {
        PageSelectForCrates(crateSelectionScreens[2], crateSelectionScreens[1]);
    }

    void PageSelectForCrates(GameObject screenToOpen, GameObject screenToClose)
    {
        screenToOpen.SetActive(true);
        screenToClose.SetActive(false);
    }

    public void CloseCrateSelection()
    {
        for (int p = 0; p <= 2; p++)
        {
            crateSelectionScreens[p].SetActive(false);
        }

        mainMenu.SetActive(true);
    }

    public void CloseStorePage()
    {
        mainMenu.SetActive(true);
        storePage.SetActive(false);
    }

    //closes level selection
    public void CloseLevelSelect()
    {
        mainMenu.SetActive(true);
        levelSelectScreen.SetActive(false);
    }

    public void OpenCreditPage()
    {
        creditPage.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseCreditPage()
    {
        creditPage.SetActive(false);
        mainMenu.SetActive(true);
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
