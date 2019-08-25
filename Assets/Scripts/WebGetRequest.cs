using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;                            

public class WebGetRequest : MonoBehaviour
{

    void Start()
    {
        string url = "http://factoryprice.co.in/best_deals/testapi.php";
        StartCoroutine(GetRequest(url));
    }



    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
       
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}
