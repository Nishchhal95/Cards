using Newtonsoft.Json;
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
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/login.php?name=" + playerName + "&email=" + playerEmail + "&pic=" + playerPic + "&imei=" + playerImei + "&coins=" + playerCoins + "&number=" + playerNumber);
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
            print("Already Available");
            onComplete?.Invoke();
        }


        if (response.Equals("New record created successfully"))
        {
            if (PlayerPrefs.GetInt("login", 0) == 0)
            {
                welcomepanel.SetActive(true);
                PlayerPrefs.SetInt("login", 1);
            }
            Debug.Log("Login Succesful!");
            onComplete?.Invoke();
        }
        else
        {
            Debug.Log("Login Failed!");
            onError?.Invoke();
        }
    }



    public static void HttpRefilsPlayers(string postData, Action<List<PlayerData>> onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpRefilsPlayersRoutine(postData, onComplete, onError));
    }

    private IEnumerator HttpRefilsPlayersRoutine(string postData, Action<List<PlayerData>> onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Post("http://languagelive.xyz/casino/getPlayers.php", postData);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postData);
        unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string jsonData = unityWebRequest.downloadHandler.text;
        RefilPlayerResponse orderResponse = JsonConvert.DeserializeObject<RefilPlayerResponse>(jsonData);

        if(orderResponse.data != null || orderResponse.data.Count <= 0)
        {
            onError?.Invoke();
        }

        onComplete?.Invoke(orderResponse.data);
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
           // MainMenu.currentcoin = MainMenu.currentcoin - int.Parse(reducedCoin);
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
            MainMenu.UserCurrentChips = MainMenu.UserCurrentChips + int.Parse(addedCoin);
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
            MainMenu.UserCurrentChips = MainMenu.UserCurrentChips + coins;
            onComplete?.Invoke();
        }
        else
        {
            Debug.Log("Failed!");
            onError?.Invoke();
        }

    }

    //------------------------------------------redeem

    public static void Httpredeem(int amount, string email, int coins, long number, Action onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpredeemRoutine(amount,email,coins,number, onComplete, onError));
    }



    private IEnumerator HttpredeemRoutine(int amount, string email, int coins, long number, Action onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://languagelive.xyz/casino/redeem.php?amount=" + amount.ToString() + "&email=" + email + "&coins=" + coins.ToString() + "&number=" + number.ToString());
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

            MainMenu.UserCurrentChips = MainMenu.UserCurrentChips -  coins;
            onComplete?.Invoke();
        }
        else
        {
            Debug.Log("Failed!");
            onError?.Invoke();
        }
    }





    //BUY COINS
    public static void HttpCreateCoinsOrderID(string postData, Action<OrderResponse> onComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpHttpCreateCoinsOrderIDRoutine(postData, onComplete, onError));
    }

    private IEnumerator HttpHttpCreateCoinsOrderIDRoutine(string postData, Action<OrderResponse> onComplete, Action onError = null)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Post("http://languagelive.xyz/casino/v1/api/create-order.php", postData);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postData);
        unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
            onError?.Invoke();
        }

        string jsonData = unityWebRequest.downloadHandler.text;
        OrderResponse orderResponse = JsonConvert.DeserializeObject<OrderResponse>(jsonData);

        onComplete?.Invoke(orderResponse);
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


[System.Serializable]
public class OrderMessage
{
    public string email;
    public string name;
    public string amount;
}

[System.Serializable]
public class OrderResponse
{
    public string status;
    public int code;
    public string msg;
    public Data data;
}

[System.Serializable]
public class Data
{
    public string id;
    public int amount;
    public string currency;
    public string receipt;
    public string status;
    public int attempts;
    public string created_at;
    public string name;
    public string email;
}

[System.Serializable]
public class RefilPlayerMessage
{
    public int minimumBet;
    public int noOfPlayers;
}

[System.Serializable]
public class PlayerData
{
    public string sno;
    public string name;
    public string email;
    public string pic;
    public string imei;
    public string coins;
    public string number;
}

[System.Serializable]
public class RefilPlayerResponse
{
    public string status;
    public int code;
    public string msg;
    public List<PlayerData> data;
}


