﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Variables
    [Header("Stars Variables")]
    public Image star1;
    public Image star2;
    public Image star3;
    public Image star1Win;
    public Image star2Win;
    public Image star3Win;
    public Sprite yellowStar;
    public Sprite nullStar;

    [Header("Lives System Variables")]
    public static int liveCount = 0;
    public Text liveCountText;

    [Header("PlayerPrefs Variables")]
    public string nameOfLevel;
    string stars;
    string highScore;

    [Header("Audio Source Variables")]
    public AudioSource clappingSound;

    [Header("Boolean Variables")]
    public static bool runGame = true;
    public bool turnOnEffects = true;

    [Header("Screens")]
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject unlockCrateScreen;
    public static bool gameIsLost = false;

    [Header("Crate Unlock Variables")]
    public Text crateUnlockText;
    public Image crateUnlockImg;

    [Header("Stars Variables")]
    public static int totalStarCount;
    public GameObject starsHolder;
    public int maxStars;
    public int starCount;

    [Header("Balloon Variable")]
    public GameObject[] balloon;
    public Transform[] balloonOrigPos;
    public HingeJoint2D[] theRopeConnect;
    Balloon balloonScript;

    [Header("Crate Types Variables")]
    public GameObject[] crates;

    AdController theAdsController;
    WindSwipe playerSwipe;
    CameraController theCam;
    StarScript theStars;
    #endregion

    private void Awake()
    {
        runGame = true;
        turnOnEffects = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelStrings();
        starsHolder.SetActive(true);

        starCount = 0;

        PlayerPrefs.GetInt(highScore); //get the max stars count for this level
        PlayerPrefs.GetInt("TypeOfCrate"); //get the type of crate selected

        theAdsController = FindObjectOfType<AdController>();
        playerSwipe = FindObjectOfType<WindSwipe>();
        theStars = FindObjectOfType<StarScript>();
        theCam = FindObjectOfType<CameraController>();
        balloonScript = FindObjectOfType<Balloon>();

        //by default, win and lose screens are set inactive at start
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void Update()
    {
        SwitchCrate();
        UpdateStars();

        liveCountText.text = ": " + liveCount;
    }

    public void SwitchCrate()
    {
        switch (PlayerPrefs.GetInt("TypeOfCrate"))
        {
            case 0:
                crates[0].SetActive(true);
                crates[1].SetActive(false);
                crates[2].SetActive(false);
                break;

            case 1:
                crates[0].SetActive(false);
                crates[1].SetActive(true);
                crates[2].SetActive(false);
                break;

            case 2:
                crates[0].SetActive(false);
                crates[1].SetActive(false);
                crates[2].SetActive(true);
                break;
        }
    }

    public void UnlockCrate(CrateScriptableObject crate)
    {
        unlockCrateScreen.SetActive(true);

        crateUnlockImg.sprite = crate.crateSprite;
        crateUnlockText.text = crate.nameOfCrate + " Unlocked";
    }

    public void CloseUnlockScreen()
    {
        unlockCrateScreen.SetActive(false);

       // Time.timeScale = 1f; //if animation does not work, this is problem
    }

    #region Audio Functions
    public static void PlayAudioSource(AudioSource audioSource)
    {
        audioSource.Play();
    }
    #endregion

    #region String Function
    void LevelStrings()
    {
        stars += nameOfLevel + " Stars";
        highScore += "Highscore of " + nameOfLevel;
    }

    void AssignVariables()
    {
        star1 = GameObject.Find("Star1").GetComponent<Image>();
        star2 = GameObject.Find("Star2").GetComponent<Image>();
        star3 = GameObject.Find("Star3").GetComponent<Image>();

        star1Win = GameObject.Find("Star1 (Win)").GetComponent<Image>();
        star2Win = GameObject.Find("Star2 (Win)").GetComponent<Image>();
        star3Win = GameObject.Find("Star3 (Win)").GetComponent<Image>();

        liveCountText = GameObject.Find("LiveCountText").GetComponent<Text>();
    }
    #endregion

    #region Highscore Function
    public void CheckHighscore()
    {
        //check if the starcount is more than max stars or highscore in the current level
        if (starCount > PlayerPrefs.GetInt(highScore, 0))
        {
            PlayerPrefs.SetInt(highScore, maxStars);
            maxStars = starCount;
        }
    }
    #endregion

    #region Update Stars Count Functions
    public void UpdateStars()
    {
        //set the starsCount to be the same as the cases
        switch (starCount)
        {
            case 0:
                star1.sprite = nullStar;
                star2.sprite = nullStar;
                star3.sprite = nullStar;
                break;

            case 1:
                star1.sprite = yellowStar;
                star2.sprite = nullStar;
                star3.sprite = nullStar;
                break;

            case 2:
                star1.sprite = yellowStar;
                star2.sprite = yellowStar;
                star3.sprite = nullStar;
                break;

            case 3:
                star1.sprite = yellowStar;
                star2.sprite = yellowStar;
                star3.sprite = yellowStar;
                break;

            default:
                star1.sprite = nullStar;
                star2.sprite = nullStar;
                star3.sprite = nullStar;
                break;
        }
    }

    //update the win screen stars count
    public void UpdateWinScreen()
    {
        switch (starCount)
        {
            case 0:
                star1Win.sprite = nullStar;
                star2Win.sprite = nullStar;
                star3Win.sprite = nullStar;

                PlayerPrefs.SetInt(stars, starCount);
                break;

            case 1:
                star1Win.sprite = yellowStar;
                star2Win.sprite = nullStar;
                star3Win.sprite = nullStar;

                PlayerPrefs.SetInt(stars, starCount);
                break;

            case 2:
                star1Win.sprite = yellowStar;
                star2Win.sprite = yellowStar;
                star3Win.sprite = nullStar;

                PlayerPrefs.SetInt(stars, starCount);
                break;

            case 3:
                star1Win.sprite = yellowStar;
                star2Win.sprite = yellowStar;
                star3Win.sprite = yellowStar;

                PlayerPrefs.SetInt(stars, starCount);
                break;

            default:
                star1Win.sprite = nullStar;
                star2Win.sprite = nullStar;
                star3Win.sprite = nullStar;

                PlayerPrefs.SetInt(stars, starCount);
                break;
        }
    }
    #endregion

    #region Screen and Changing Scene Functions
    public void LoseGame()
    {
            gameIsLost = true;
            loseScreen.SetActive(true);
            theCam.followPlayer = false;
            starsHolder.SetActive(false);
    }

    public void ResetGame()
    {
        if (liveCount > 0)
        {
            liveCount -= 1;
            balloon[PlayerPrefs.GetInt("TypeOfCrate")].SetActive(true);
            balloon[PlayerPrefs.GetInt("TypeOfCrate")].GetComponent<Animator>().SetTrigger("ResetBalloon");
            runGame = true;
            theCam.followPlayer = true;
            starsHolder.SetActive(true);
            LevelManager.gameIsLost = false;
            loseScreen.SetActive(false);
        } else
        {
            //watch Ad
            theAdsController.PlayRewardedAd();
        }
    }

    public void BalloonAnimation()
    {
        balloon[PlayerPrefs.GetInt("TypeOfCrate")].GetComponent<Balloon>().hasPop = false;
        balloon[PlayerPrefs.GetInt("TypeOfCrate")].transform.position = balloonOrigPos[PlayerPrefs.GetInt("TypeOfCrate")].position;
        theRopeConnect[PlayerPrefs.GetInt("TypeOfCrate")].connectedAnchor = new Vector2(0.001000404f, 0.2980003f);
    }

    public void WinGame()
    {
        PlayAudioSource(clappingSound);
        winScreen.SetActive(true);
        starsHolder.SetActive(false);
        Debug.Log("win is on");
        runGame = false;
    }

    public void RestartGame()
    {
        turnOnEffects = false;
        Scene currentScene = SceneManager.GetActiveScene(); //get the current active scene

        gameIsLost = false;
        //LevelManager.runGame = true;

        SceneManager.LoadScene(currentScene.name); //load the current scene by its name
        Time.timeScale = 1f; //reset the time scale back to 1
    }

    public void BackToMain()
    {
        turnOnEffects = false;
        SceneManager.LoadScene("Main Menu"); //return to main menu
        Time.timeScale = 1f;
    }

    public void Continue(string nextLevelName)
    {
        turnOnEffects = false;
        SceneManager.LoadScene(nextLevelName); //load next level scene by its name
        Time.timeScale = 1f;
    }
    #endregion
}

