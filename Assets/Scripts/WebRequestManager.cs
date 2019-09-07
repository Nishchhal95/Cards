using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : MonoBehaviour
{
    public static WebRequestManager Instance = null;

    private void Awake()
    {
        if(Instance == null)
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
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/getcoin.php?email="+ playerEmail);
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
}

public class CoinList
{
    public List<Result> result { get; set; }
}

public class Result
{
    public string coins { get; set; }
}
