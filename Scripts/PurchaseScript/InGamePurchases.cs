using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePurchases : MonoBehaviour
{
    public static int inGameCurrency = 0;

    public void WatchAdFor2Lives()
    {
        FindObjectOfType<AdController>().PlayRewardedAd();
    }

    public void BuyLives10()
    {
        AddLives(10, 5);
    }

    public void BuyLives50()
    {
        AddLives(50, 45);
    }

    public void BuyLives100()
    {
        AddLives(100, 80);
    }

    public void BuySeeds10()
    {
        AddSeeds(10);
    }

    public void BuySeeds50()
    {
        AddSeeds(50);
    }

    public void BuySeeds100()
    {
        AddSeeds(100);
    }

    public void AddLives(int livesToAdd, int moneyToDeduct)
    {
        //check if player have enough seeds to buy lives
        if (InGamePurchases.inGameCurrency >= moneyToDeduct)
        {
            //buy livecounts with seeds
            LevelManager.liveCount += livesToAdd;
            InGamePurchases.inGameCurrency -= moneyToDeduct;
        }
    }

    public void AddSeeds(int moneyToAdd)
    {
        //buy with REAL money for seeds
        InGamePurchases.inGameCurrency += moneyToAdd;
    }
}
