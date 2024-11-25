using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    [SerializeField] private string _androidID = "5736769";
    [SerializeField] private bool _testingMode;
    [SerializeField] string _adUnitId = "Interstitial_Android";

    /*private void Awake()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_androidID, _testingMode, this);
        }
    }*/

    private void Start()
    {
        Advertisement.Initialize(_androidID, _testingMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Initialization success");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Initialization Failure");
    }

    public void ShowAd()
    {
        if (!Advertisement.isInitialized) return;
        Advertisement.Show(_adUnitId, this);
        
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        GameManager.Instance.SaveGame();
        Debug.Log("comenzó publicidad");
    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Show complete");
            GameManager.Instance.LoadGame();
        }
        else
        {
            Debug.Log("Show failed");
        }
    }
}
