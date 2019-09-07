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

          //  Debug.Log(www.downloadHandler.text);  // prints data from the url

            jsonstring = www.downloadHandler.text;




            // JSONArray array = (JSONArray)jsonObject.get("result");

       

            List<results> r = JsonConvert.DeserializeObject <List<results>>(jsonstring);

            
            


            for (int i = 0; i < r.Count; i++)
            {


                Debug.Log("Dynamics coin fetched.........");
                Debug.Log(r[i].coins +"coins fetched");
              


            }

        }

    }

          public class results


    {
        public string coins;
      


    }



}









