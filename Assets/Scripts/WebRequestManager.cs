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
}
