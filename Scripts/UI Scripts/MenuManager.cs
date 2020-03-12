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
    public Animator loadingScreen;

    [Header("Text Variables")]
    public Text seedCountText;
    public Text liveCountText;

    private void Start()
    {
        //close level select and Credit Page and open main menu by default
        /*CloseLevelSelect(); 
        CloseCreditPage();*/
        OpenStorePage();
        clearPlayerprefsScreen.SetActive(false);
    }

    private void Update()
    {
        seedCountText.text = "Paw Coins: " + InGamePurchases.inGameCurrency;
        liveCountText.text = "Lives: " + LevelManager.liveCount;
    }

    public void StartGame()
    {
        //load level 1 for now
        SceneManager.LoadScene("GameScene");
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
