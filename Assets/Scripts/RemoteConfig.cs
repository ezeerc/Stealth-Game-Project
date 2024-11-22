using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class RemoteConfig : MonoBehaviour
{
    public struct userAttributes {}
    public struct appAttributes {}

    public TextMeshProUGUI textWelcome;
    public TextMeshProUGUI version;
    public GameManager gameManager;
    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async Task Start()
    {
        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());
        textWelcome.text = RemoteConfigService.Instance.appConfig.config.Value<String>("welcomeText");
        version.text = RemoteConfigService.Instance.appConfig.config.Value<int>("version").ToString();
        gameManager.speed = RemoteConfigService.Instance.appConfig.config.Value<int>("speed");
        /*gameManager.InstantietePrefab(new Vector3(
            RemoteConfigService.Instance.appConfig.config.Value<float>("initialPos.x"),
            RemoteConfigService.Instance.appConfig.config.Value<float>("initialPos.y"),
            RemoteConfigService.Instance.appConfig.config.Value<float>("initialPos.z")));*/
    }

}
