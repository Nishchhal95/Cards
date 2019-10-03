using UnityEngine;

using GoogleMobileAds;
using GoogleMobileAds.Api;
using System.Collections;
using System;

public class RewardGame : MonoBehaviour {

	private RewardBasedVideoAd rewardBasedVideoAd;

	private static string outputMessage = "";

	public static string OutputMessage
	{
		set { outputMessage = value; }
	}

	public void Start()
	{
		rewardBasedVideoAd = RewardBasedVideoAd.Instance;

		rewardBasedVideoAd.OnAdLoaded += HandleOnAdLoaded;
		rewardBasedVideoAd.OnAdFailedToLoad += HandleOnFailedToLoad;
		rewardBasedVideoAd.OnAdOpening += HandleOnOpening;
		rewardBasedVideoAd.OnAdStarted += HandleOnStarted;
		rewardBasedVideoAd.OnAdClosed += HandleOnAdClosed;
		rewardBasedVideoAd.OnAdRewarded += HandleOnAdRewarded;
		rewardBasedVideoAd.OnAdLeavingApplication += HandleIOnAdLeavingApplication;

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
		    print("Reward is not ready yet.");
		}
	}

		#region Reward callback handlers

		public void HandleOnAdLoaded(object sender, EventArgs args)
		{
		 
		}

		public void HandleOnFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
		  
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
		   
		}

		public void HandleOnAdRewarded(object sender, Reward args)
		{
		  print("Reward HERE");
          MainMenu.UserCurrentDiamonds += 2000;
        //manager.Coins += 2000;
		 
		}

		public void HandleIOnAdLeavingApplication(object sender, EventArgs args)
		{
		print("Leave Application");
		}

		#endregion

}
