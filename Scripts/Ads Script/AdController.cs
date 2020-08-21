using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;

public class AdController : MonoBehaviour
{
    string gameId = "3779927";
    string rewardedVideoId = "rewardedVideo";

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        //if game is released, put it to false
        Advertisement.Initialize(gameId, true);   

    }

    public void PlayRewardedAd()
    {
        //is the rewarded video ad ready to be played
        if (Advertisement.IsReady(rewardedVideoId))
        {
            Advertisement.Show(rewardedVideoId);
            //InGamePurchases.inGameCurrency += 10;
            LevelManager.liveCount += 2;
        }
    }

    public void PlaySimpleAd() {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        }
    }
}
