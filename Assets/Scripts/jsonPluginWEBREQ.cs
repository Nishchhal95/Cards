using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class jsonPluginWEBREQ : MonoBehaviour
{

    string jsonstring;


    void Start()


    {

        StartCoroutine(GetText());


    }


    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://factoryprice.co.in/best_deals/getplayer.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);

        }
        else
        {

            Debug.Log(www.downloadHandler.text);  // prints data from the url

            jsonstring = www.downloadHandler.text;


            List<playerData> playerList = JsonConvert.DeserializeObject<List<playerData>>(jsonstring);


           // Debug.Log(playerList);
           

            for (int i=0;i<playerList.Count;i++)
            {
                Debug.Log(playerList[i].name);
                Debug.Log(playerList[i].coin);
                Debug.Log(playerList[i].email);
            }

        }




    }


    

    public class playerData


    {
        public int coin;
        public string name;
        public string email;
      

    }





}
