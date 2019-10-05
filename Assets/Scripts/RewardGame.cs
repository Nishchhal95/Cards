using UnityEngine;

using GoogleMobileAds;
using GoogleMobileAds.Api;
using System.Collections;
using System;

public class RewardGame : MonoBehaviour {

	private RewardBasedVideoAd rewardBasedVideoAd;

    public delegate void OnDiamondsChanged();
    public static OnDiamondsChanged onDiamondsChanged;

	private static string outputMessage = "";
    MainMenu menuScript;

	public static string OutputMessage
	{
		set { outputMessage = value; }
	}

	public void Start()
	{
        menuScript = FindObjectOfType<MainMenu>();
        MobileAds.Initialize(initStatus => {
            RequestRewardBasedAd();
        });

        rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        rewardBasedVideoAd.OnAdLoaded += HandleOnAdLoaded;
        rewardBasedVideoAd.OnAdFailedToLoad += HandleOnFailedToLoad;
        rewardBasedVideoAd.OnAdOpening += HandleOnOpening;
        rewardBasedVideoAd.OnAdStarted += HandleOnStarted;
        rewardBasedVideoAd.OnAdClosed += HandleOnAdClosed;
        rewardBasedVideoAd.OnAdRewarded += RewardBasedVideoAd_OnAdRewarded;
        rewardBasedVideoAd.OnAdLeavingApplication += HandleIOnAdLeavingApplication;
    }

    private void RewardBasedVideoAd_OnAdRewarded(object sender, Reward e)
    {
        Debug.Log("Reward HERE");

        int coins = MainMenu.UserCurrentChips + 2000;
        MainMenu.UserCurrentChips = coins;
        Debug.Log(FB_Handler.instance.SavedEmail + coins);

        menuScript.SetChipsText();
        WebRequestManager.HttpGetAddCoin(FB_Handler.instance.SavedEmail, 2000.ToString(), () =>
        {
            Debug.Log("Added win coin");
        });

        RequestRewardBasedAd();
    }

    void Update()
	{
		//	ShowRewardBasedAd();
	}

	public void RequestRewardBasedAd()
	{
#if UNITY_EDITOR
		string adUnitId = "ca-app-pub-6164780910839363/9417809966";
#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-6164780910839363/9417809966";
#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Register for ad events.

        // Load an reward ad.
        //rewardBasedVideoAd.LoadAd(new AdRequest.Builder().Build(), adUnitId);

        if(rewardBasedVideoAd != null)
        rewardBasedVideoAd.LoadAd(new AdRequest.Builder().Build(), adUnitId);
	}

	public void ShowRewardBasedAd()
	{
		if (rewardBasedVideoAd.IsLoaded())
		{
            rewardBasedVideoAd.Show();
		}
		else
		{
            RequestRewardBasedAd();
            print("Reward is not ready yet.");
		}
	}

		#region Reward callback handlers

		public void HandleOnAdLoaded(object sender, EventArgs args)
		{
		    
		}

		public void HandleOnFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
            Debug.Log("Reward AD Failed to Load");
        }

		public void HandleOnOpening(object sender, EventArgs args)
		{
		    print("Opening");
		}

		public void HandleOnStarted(object sender, EventArgs args)
		{
		print("Started");
		}

		public void HandleOnAdClosed(object sender, EventArgs args)
		{
            RequestRewardBasedAd();
        }

		public void HandleOnAdRewarded(object sender, Reward args)
		{


        }

		public void HandleIOnAdLeavingApplication(object sender, EventArgs args)
		{
		print("Leave Application");
		}

		#endregion

}
