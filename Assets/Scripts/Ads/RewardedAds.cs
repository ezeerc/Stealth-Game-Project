using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string _rewardedAdID = "Rewarded_Anbdroid";

    public void LoadRewardedAd()
    {
        Advertisement.Load(_rewardedAdID, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(_rewardedAdID, this);
        LoadRewardedAd();
    }
    
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded loadad");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Rewarded loading failure");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Rewarded sad clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == _rewardedAdID)
        {
            Debug.Log("Rewarded ad completed");
            if(showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) Debug.Log("Rewarded ad completed");
            else if(showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED)) Debug.Log("Rewarded ad skipped");
            else if(showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN)) Debug.Log("Error");
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Rewarded ad show failure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Rewarded ad start");
    }
}
