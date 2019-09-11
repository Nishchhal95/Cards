using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class jsonPluginWEBREQ : MonoBehaviour
{
    public static jsonPluginWEBREQ Instance = null;
    public int coins;

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

    private void Start()
    {

    }


    string jsonstring;


    public string coinsFetched;

    public void initData(string imei)
    {
        // TO GET PLAYER LIST----------------------------------------------------------
        WebRequestManager.HttpGetPlayerData((List<GameNameSpace.Player> playerList) =>
        {


        });

        //TO LOGIN---------------------------------------------------------------------------------------------------------------
        WebRequestManager.HttpGetPlayerLoginData(FB_Handler.instance.FB_UserName.text,FB_Handler.instance.FB_Email.text, "https"+"://graph.facebook.com/" + FB_Handler.instance.SavedId + "/picture?type=large", imei, "5000", "", () =>
         {
            Debug.Log("CREATED USER SUCCESFULLY");
           
         });



        WebRequestManager.HttpGetPlayerCoinsData(FB_Handler.instance.FB_Email.text, (string coins) =>
        {
            MainMenu.currentcoin = int.Parse(coins);
            Debug.Log(FB_Handler.instance.FB_Email.text + coins);
        });
        //TO GET COINS-------------------------------------------------------------------


    }

    public void getcoins(int coin)
    {
        coins= coin;
    }

    public void buycoins(int amount)
    {
        WebRequestManager.HttpBuyCoin(amount, FB_Handler.instance.FB_Email.text, coins, FB_Handler.instance.FB_Email.text, () =>
           {
               Debug.Log("buy");
           });

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


            for (int i = 0; i < playerList.Count; i++)
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
