using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFlag : MonoBehaviour
{
    LevelManager theLM;

    LevelSelection levelSelect;
    UnlockCrate theUnlockableCrate;

    public int levelToUnlocked;

    private void Start()
    {
        theLM = FindObjectOfType<LevelManager>();
        levelSelect = FindObjectOfType<LevelSelection>();
        theUnlockableCrate = FindObjectOfType<UnlockCrate>();

        if (!theUnlockableCrate)
        {
            Debug.Log("cant find crate");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            if (!LevelManager.gameIsLost)
            {
                WinGame();
            }
        }
    }

    public void WinGame()
    {
        theLM.UpdateWinScreen();
        theLM.CheckHighscore();
        UnlockCrate();

        PlayerPrefs.SetInt("Level Unlocked", levelToUnlocked); 
        theLM.WinGame();
    }

    void UnlockCrate()
    {
        PlayerPrefs.SetInt(theUnlockableCrate.playerprefName, 1);
    }
}
