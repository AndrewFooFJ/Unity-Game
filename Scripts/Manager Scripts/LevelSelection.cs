using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    int levelNum;

    public int[] starsHighscore;

    public Sprite[] starsAmt;
    public Image[] levelSpriteIndicator;

    public GameObject[] unlockedLevels;
    public GameObject[] lockedLevels;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefsFunctions();

        UpdateLevels();

        for (int levelNum = 1; levelNum <= 20; levelNum++)
        {
            CheckStarCount("Level " + levelNum + " Stars", "Max Level " + levelNum + " Stars");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevels();
        PlayerPrefsFunctions();
    }

    #region PlayerPrefs Starting Functions
    void PlayerPrefsFunctions()
    {
        PlayerPrefs.GetInt("Max Levels");

        //if the level unlock is more, override the max levels and set to that number
        if (PlayerPrefs.GetInt("Level Unlocked", levelNum) > PlayerPrefs.GetInt("Max Levels"))
        {
            PlayerPrefs.SetInt("Max Levels", PlayerPrefs.GetInt("Level Unlocked")); //set the max num to be the same as the level unlocked
        }

        //if max levels is more, override the level num to be always be the same as max levels
        if (PlayerPrefs.GetInt("Max Levels") > PlayerPrefs.GetInt("Level Unlocked", levelNum))
        {
            levelNum = PlayerPrefs.GetInt("Max Levels");
            PlayerPrefs.SetInt("Level Unlocked", levelNum);
        }

        //retreive the number of stars which player has gotten on each level
        PlayerPrefs.GetInt("Level 1 Stars", starsHighscore[0]);
        PlayerPrefs.GetInt("Level 2 Stars", starsHighscore[1]);
        PlayerPrefs.GetInt("Level 3 Stars", starsHighscore[2]);
        PlayerPrefs.GetInt("Level 4 Stars", starsHighscore[3]);
        PlayerPrefs.GetInt("Level 5 Stars", starsHighscore[4]);
        PlayerPrefs.GetInt("Level 6 Stars", starsHighscore[5]);
        PlayerPrefs.GetInt("Level 7 Stars", starsHighscore[6]);
        PlayerPrefs.GetInt("Level 8 Stars", starsHighscore[7]);
        PlayerPrefs.GetInt("Level 9 Stars", starsHighscore[8]);
        PlayerPrefs.GetInt("Level 10 Stars", starsHighscore[9]);
        PlayerPrefs.GetInt("Level 11 Stars", starsHighscore[10]);
        PlayerPrefs.GetInt("Level 12 Stars", starsHighscore[11]);
        PlayerPrefs.GetInt("Level 13 Stars", starsHighscore[12]);
        PlayerPrefs.GetInt("Level 14 Stars", starsHighscore[13]);
        PlayerPrefs.GetInt("Level 15 Stars", starsHighscore[14]);
        PlayerPrefs.GetInt("Level 16 Stars", starsHighscore[15]);
        PlayerPrefs.GetInt("Level 17 Stars", starsHighscore[16]);
        PlayerPrefs.GetInt("Level 18 Stars", starsHighscore[17]);
        PlayerPrefs.GetInt("Level 19 Stars", starsHighscore[18]);
        PlayerPrefs.GetInt("Level 20 Stars", starsHighscore[19]);

        //check if less stars does not override the max stars collected for each level
        for (int levelNum = 1; levelNum <= 20; levelNum++)
        {
            CheckStarCount("Level " + levelNum + " Stars", "Max Level " + levelNum + " Stars");
        }

        //update the stars based on each level collection of stars
        UpdateStars("Level 1 Stars", 0);
        UpdateStars("Level 2 Stars", 1);
        UpdateStars("Level 3 Stars", 2);
        UpdateStars("Level 4 Stars", 3);
        UpdateStars("Level 5 Stars", 4);
        UpdateStars("Level 6 Stars", 5);
        UpdateStars("Level 7 Stars", 6);
        UpdateStars("Level 8 Stars", 7);
        UpdateStars("Level 9 Stars", 8);
        UpdateStars("Level 10 Stars", 9);
        UpdateStars("Level 11 Stars", 10);
        UpdateStars("Level 12 Stars", 11);
        UpdateStars("Level 13 Stars", 12);
        UpdateStars("Level 14 Stars", 13);
        UpdateStars("Level 15 Stars", 14);
        UpdateStars("Level 16 Stars", 15);
        UpdateStars("Level 17 Stars", 16);
        UpdateStars("Level 18 Stars", 17);
        UpdateStars("Level 19 Stars", 18);
        UpdateStars("Level 20 Stars", 19);
    }
    #endregion

    #region Check Star Count Functions
    void CheckStarCount(string levelStars, string maxLevelStars)
    {
        Debug.Log(PlayerPrefs.GetInt(levelStars) + " is the current stars, " + levelStars);
        Debug.Log(PlayerPrefs.GetInt(maxLevelStars) + " is the max current stars, " + maxLevelStars);

        //if level stars is more, set the max level stars to be level stars
        if (PlayerPrefs.GetInt(levelStars) > PlayerPrefs.GetInt(maxLevelStars))
        {
            PlayerPrefs.SetInt(maxLevelStars, (PlayerPrefs.GetInt(levelStars)));
        }

        PlayerPrefs.SetInt(levelStars, PlayerPrefs.GetInt(maxLevelStars)); //set the level stars to be max level stars
    }

    //update the number of stars gotten on each level
    void UpdateStars(string levelStars, int levelNum)
    {
        if (PlayerPrefs.GetInt(levelStars, starsHighscore[levelNum]) == 0)
        {
            levelSpriteIndicator[levelNum].sprite = starsAmt[0];
        }

        if (PlayerPrefs.GetInt(levelStars, starsHighscore[levelNum]) == 1)
        {
            levelSpriteIndicator[levelNum].sprite = starsAmt[1];
        }

        if (PlayerPrefs.GetInt(levelStars, starsHighscore[levelNum]) == 2)
        {
            levelSpriteIndicator[levelNum].sprite = starsAmt[2];
        }

        if (PlayerPrefs.GetInt(levelStars, starsHighscore[levelNum]) >= 3)
        {
            levelSpriteIndicator[levelNum].sprite = starsAmt[3];
        }
    }
    #endregion

    #region Level Unlock Functions
    //unlock the levels 
    void UnlockedLevels(GameObject unlockedLvl, GameObject lockedLvl)
    {
        unlockedLvl.SetActive(true);
        lockedLvl.SetActive(false);
    }

    //updates the levels that have been unlocked
    public void UpdateLevels()
    {
        int numOfLevelsUnlocked = PlayerPrefs.GetInt("Level Unlocked");

        switch (numOfLevelsUnlocked)
        {
            case 0:
                UnlockedLevels(unlockedLevels[0], lockedLevels[0]);
                break;

            case 1:
                for (int num = 0; num < 2; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 2:
                for (int num = 0; num < 3; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 3:
                for (int num = 0; num < 4; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 4:
                for (int num = 0; num < 5; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 5:
                for (int num = 0; num < 6; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 6:
                for (int num = 0; num < 7; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 7:
                for (int num = 0; num < 8; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 8:
                for (int num = 0; num < 9; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 9:
                for (int num = 0; num < 10; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 10:
                for (int num = 0; num < 11; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 11:
                for (int num = 0; num < 12; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 12:
                for (int num = 0; num < 13; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 13:
                for (int num = 0; num < 14; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 14:
                for (int num = 0; num < 15; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 15:
                for (int num = 0; num < 16; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 16:
                for (int num = 0; num < 17; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 17:
                for (int num = 0; num < 18; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 18:
                for (int num = 0; num < 19; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 19:
                for (int num = 0; num < 20; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            case 20:
                for (int num = 0; num < 22; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;

            default:
                for (int num = 0; num < 22; num++)
                {
                    UnlockedLevels(unlockedLevels[num], lockedLevels[num]);
                }
                break;
        }
    }
    #endregion
}
