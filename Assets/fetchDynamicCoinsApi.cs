using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class fetchDynamicCoinsApi : MonoBehaviour
{
    // Start is called before the first frame update
    string jsonstring;

    void Start()
    {


        StartCoroutine(GetText());

    }


    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://languagelive.xyz/casino/getcoin.php");   //dynamic coin fetch
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);

        }
        else
        {

            Debug.Log(www.downloadHandler.text);  // prints data from the url

            jsonstring = www.downloadHandler.text;


            List<getcoin> g= JsonConvert.DeserializeObject<List<getcoin>>(jsonstring);


            // Debug.Log(playerList);


            for (int i = 0; i < g.Count; i++)
            {


                Debug.Log("Dynamics coin fetched.........");
                Debug.Log(g[i].coins);
              


            }

        }

    }

          public class getcoin


    {
        public int coins;
      


    }



}









