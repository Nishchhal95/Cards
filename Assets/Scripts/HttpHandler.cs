using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// CREATE HttpRequest<T> object where T is gonna be your class of Request Type(Request Parameters should be in this Class and this class
/// should override ToString()
/// 
/// Example---------------------------------------------------------------------
/// [System.Serializable]
///public class LoginMessage
///{
///    public string emailId;
///    public int coins;
///    public float XP;

/// public override string ToString()
/// {
///        return "emailId=" + emailId.ToString() + "&"
///            + "coins=" + coins.ToString() + "&"
///             + "XP=" + XP.ToString();
///    }
///}
///-------------------------------------------------------------------------
/// [System.Serializable]
///public class LoginResponse
///{
///    public string responseMessage;
///    public int statusCode;
///    public string status;
///} 
///
/// LoginMessage loginMessage = new LoginMessage
/// {
///     emailId = "abc@xyz.com",
///     points = 100,
///     XP = 200f
/// }
/// -------------------------------------------------------------------------
/// HttpRequest<LoginMessage> httpRequest = new HttpRequest(loginMessage, URL);
/// 
/// HttpHandler.Get(httpRequest, (LoginResponse loginResponse) => 
/// { 
///     Debug.Log("Resposne: " + loginResponse.status); 
/// }, () => 
///     {
///         Debug.Log("Failed");
///     });
/// 
/// </summary>

public class HttpHandler : MonoBehaviour
{
    public static HttpHandler Instance = null;

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

    public static void Get<T, U>(HttpRequest<U> httpRequest, Action<T> onRequestComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpGetRoutine(httpRequest, onRequestComplete, onError));
    }

    public static void Post<T, U>(HttpRequest<U> httpRequest, Action<T> onRequestComplete, Action onError = null)
    {
        Instance.StartCoroutine(Instance.HttpPostRoutine(httpRequest, onRequestComplete, onError));
    }

    private IEnumerator HttpGetRoutine<T, U>(HttpRequest<U> httpRequest, Action<T> onRequestComplete, Action onError = null)
    {
        Utils.ColorLog("cyan", "Making Http Request at URI : " + httpRequest.URI + httpRequest.message);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(httpRequest.URI + httpRequest.message);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError)
        {
            Utils.ColorLog("red", "Network Error");
            onError?.Invoke();
            yield break;
        }

        if (unityWebRequest.isHttpError)
        {
            Utils.ColorLog("red", "Http Error");
            onError?.Invoke();
            yield break;
        }
        
        string json = unityWebRequest.downloadHandler.text;
        Utils.ColorLog("green", "Http Response " + json);
        T reseponse = JsonConvert.DeserializeObject<T>(json);
        onRequestComplete?.Invoke(reseponse);
    }

    private IEnumerator HttpPostRoutine<T, U>(HttpRequest<U> httpRequest, Action<T> onRequestComplete, Action onError = null)
    {
        Utils.ColorLog("cyan", "Making Http Request at URI : " + httpRequest.URI + httpRequest.message);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(httpRequest.URI, httpRequest.message);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(httpRequest.message);
        unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError)
        {
            Utils.ColorLog("red", "Network Error");
            onError?.Invoke();
            yield break;
        }

        if (unityWebRequest.isHttpError)
        {
            Utils.ColorLog("red", "Http Error");
            onError?.Invoke();
            yield break;
        }

        string json = unityWebRequest.downloadHandler.text;
        Utils.ColorLog("green", "Http Response " + json);
        T response = JsonConvert.DeserializeObject<T>(json);
        onRequestComplete?.Invoke(response);
    }
}

[Serializable]
public class HttpRequest<T>
{
    public string URI ="";
    public string message;

    public HttpRequest(T message, string uri)
    {
        this.message = message.ToString();
        URI = uri;
    }  
}
