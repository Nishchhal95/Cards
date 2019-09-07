﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class fetchDynamicCoinsApi : MonoBehaviour
{
    // Start is called before the first frame update
    string jsonstring;

    string coinsFetched;

    void Start()
    {

        //TO GET COINS-------------------------------------------------------------------
        WebRequestManager.HttpGetPlayerCoinsData("nishchhal@xyz.com", (string coins) =>
        {
        Debug.Log("surbhishukla coins " + coins);


            coinsFetched = coins;// storing in string
         });

        StartCoroutine(GetText());

    }



    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://languagelive.xyz/casino/getcoin.php?email=surbhishukla38@yahoo.com");   //dynamic coin fetch
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);

        }
        else
        {

            Debug.Log(www.downloadHandler.text);  // prints data from the url

            jsonstring = www.downloadHandler.text;



            Debug.Log("coins fetched from getcoin" + coinsFetched);

        }
    }

          public class results


    {
         public string coins;

      

    }



}









