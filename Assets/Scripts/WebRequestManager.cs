﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : MonoBehaviour
{
    public GameObject welcomepanel;
    public static WebRequestManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(this);
        }
    }

    public static void HttpGetPlayerData(Action<List<GameNameSpace.Player>> onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetPlayerDataRoutine(onComplete, onError));
    }

    private IEnumerator HttpGetPlayerDataRoutine(Action<List<GameNameSpace.Player>> onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://factoryprice.co.in/best_deals/getplayer.php");
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string playerDataJson = unityWebRequest.downloadHandler.text;
        List<GameNameSpace.Player> playerList = JsonConvert.DeserializeObject<List<GameNameSpace.Player>>(playerDataJson);

        onComplete?.Invoke(playerList);
    }

    public static void HttpGetPlayerCoinsData(string playerEmail, Action<string> onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetPlayerCoinsDataRoutine(playerEmail, onComplete, onError));
    }

    private IEnumerator HttpGetPlayerCoinsDataRoutine(string playerEmail, Action<string> onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/getcoin.php?email=" + playerEmail);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string coinJsonData = unityWebRequest.downloadHandler.text;
        CoinList coinList = JsonConvert.DeserializeObject<CoinList>(coinJsonData);

        onComplete?.Invoke(coinList.result[0].coins);
    }

    public static void HttpGetPlayerLoginData(string playerName, string playerEmail, string playerPic, string playerImei, string playerCoins, string playerNumber, Action onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetPlayerLoginDataRoutine(playerName, playerEmail, playerPic, playerImei, playerCoins, playerNumber, onComplete, onError));
    }

    private IEnumerator HttpGetPlayerLoginDataRoutine(string playerName, string playerEmail, string playerPic, string playerImei, string playerCoins, string playerNumber, Action onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/login.php?name=" + playerName + "&email=" + playerEmail + "&pic=" + playerEmail + "&imei=" + playerImei + "&coins=" + playerCoins + "&number=" + playerNumber);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string response = unityWebRequest.downloadHandler.text;

        if (response.Equals("Email id already registered."))
        {
            welcomepanel.SetActive(false);
            onComplete?.Invoke();
        }


        if (response.Equals("New record created successfully"))
        {
            Debug.Log("Login Succesful!");
            onComplete?.Invoke();
        }
        else
        {
            Debug.Log("Login Failed!");
            onError?.Invoke();
        }
    }



    //-------------------win api 

    public static void HttpGetPlayerWinData(string playerName, string playerEmail, string winAmount, Action onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetPlayerWinDataRoutine(playerName, playerEmail, winAmount, onComplete, onError));
    }

    //----------------- for  win api routine
    private IEnumerator HttpGetPlayerWinDataRoutine(string playerName, string playerEmail, string winAmount, Action onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/login.php?name=" + playerName + "&email=" + playerEmail + "&winamount=" + winAmount);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string response = unityWebRequest.downloadHandler.text;
        if (response.Equals("New record created successfully"))
        {
            Debug.Log("succeful windata send!");
            onComplete?.Invoke();
        }

        else
        {
            Debug.Log("Failed!");
            onError?.Invoke();
        }
    }
    //--------------




    //-------------------coin deduction API

    public static void HttpGetDeductedCoin(string playerName, string playerEmail, string DeductedCoinValue, Action onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetDeductedCoinRoutine(playerName, playerEmail, DeductedCoinValue, onComplete, onError));
    }

    //----------------- for  deducted coin  api routine
    private IEnumerator HttpGetDeductedCoinRoutine(string playerName, string playerEmail, string DeductedCoinValue, Action onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/login.php?name=" + playerName + "&email=" + playerEmail + "&deductedCoin=" + DeductedCoinValue);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string response = unityWebRequest.downloadHandler.text;
        if (response.Equals("New record created successfully"))
        {
            Debug.Log("succeful coin deduction!");
            onComplete?.Invoke();
        }

        else
        {
            Debug.Log("Failed!");
            onError?.Invoke();
        }
    }


    //------------------------coin reduction------------------------




    public static void HttpGetReductedCoin(string playerEmail, string DeductedCoinValue, Action onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetReductedCoinRoutine(playerEmail, DeductedCoinValue, onComplete, onError));
    }




    //----------------- for  reduced  coin  api routine


    private IEnumerator HttpGetReductedCoinRoutine(string playerEmail, string reducedCoin, Action onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/reduce_coin.php?coin=" + reducedCoin + "&email=" + playerEmail);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error); 
             onError?.Invoke();
        }

        string response = unityWebRequest.downloadHandler.text;
        if (response.Equals("Updated"))
        {
            Debug.Log("succeful coin reduction!");
            onComplete?.Invoke();
        }

        else
        {
            Debug.Log("Failed!");
            onError?.Invoke();
        }
    }




    //------------------------------------------add coins if player wins

    public static void HttpGetAddCoin(string playerEmail, string addedCoin, Action onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetAddCoinRoutine(playerEmail, addedCoin, onComplete, onError));
    }



    private IEnumerator HttpGetAddCoinRoutine(string playerEmail, string addedCoin, Action onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/addcoin.php?coin="+ addedCoin +"&email="  + playerEmail );
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string response = unityWebRequest.downloadHandler.text;
        if (response.Equals("Updated"))
        {
            Debug.Log("succeful add coin !");
            onComplete?.Invoke();
        }
        else
        {
            Debug.Log("Failed!");
            onError?.Invoke();
        }
    }



    //--------------------------------------------------BuyCoins
    public static void HttpBuyCoin(int amount, string playerEmail, int coins, string id, Action onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetPlayerCoinsDataRoutine(amount, playerEmail, coins, id, onComplete, onError));
    }

    private IEnumerator HttpGetPlayerCoinsDataRoutine(int amount, string playerEmail, int coins, string id, Action onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/buycoins.php?amount=" + amount.ToString() + "&email=" + playerEmail
            + "&coins=" + coins.ToString() + "&id" + id);

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string response = unityWebRequest.downloadHandler.text;

        if (response.Equals("Coins Added"))
        {
            Debug.Log("succeful added!");
            onComplete?.Invoke();
        }
        else
        {
            Debug.Log("Failed!");
            onError?.Invoke();
        }

    }

    //------------------------------------------add coins if player wins

    public static void Httpredeem(int amount, string email, int coins, int number, Action onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpredeemRoutine(amount,email,coins,number, onComplete, onError));
    }



    private IEnumerator HttpredeemRoutine(int amount, string email, int coins, int number, Action onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/Redeem.php?amount=" + amount.ToString() + "&email=" + email + "&coins=" + coins.ToString() + "&number=" + number.ToString());
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string response = unityWebRequest.downloadHandler.text;
        if (response.Equals("Redeemed Success"))
        {
            Debug.Log("succefull redeem");
            onComplete?.Invoke();
        }
        else
        {
            Debug.Log("Failed!");
            onError?.Invoke();
        }
    }



}



public class CoinList
{
    public List<Result> result { get; set; }
}

public class Result
{
    public string coins { get; set; }
}
