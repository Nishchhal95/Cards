﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class jsonPluginWEBREQ : MonoBehaviour
{
    public static jsonPluginWEBREQ Instance = null;
    public int coins;

    public GameObject Menu;
    public GameObject LoadingBar;

    private MainMenu mainmenuscript;
    private LoadingScript loadingscript;

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

    public void Start()
    {
         mainmenuscript = Menu.GetComponent<MainMenu>();
         loadingscript = LoadingBar.GetComponent<LoadingScript>();

        //PLAYER REFIL--------------------------------------------------------

       /* RefilPlayerMessage refilPlayerMessage = new RefilPlayerMessage
        {
            minimumBet = 10,
            noOfPlayers = 3
        };

        string json = JsonConvert.SerializeObject(refilPlayerMessage);

        WebRequestManager.HttpRefilsPlayers(json, (List<PlayerData> playerDataList) =>
        {
            for (int i = 0; i < playerDataList.Count; i++)
            {
                Debug.Log("Player Refil Item " + playerDataList[i].name);
            }
        });*/

        //PLAYER REFIL--------------------------------------------------------
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
        WebRequestManager.HttpGetPlayerLoginData(FB_Handler.instance.SavedUsername,FB_Handler.instance.SavedEmail, "https"+"://graph.facebook.com/" + FB_Handler.instance.SavedId + "/picture?type=large", imei, "5000", "", () =>
         {
             Debug.Log("CREATED USER SUCCESFULLY");
           
         });



        WebRequestManager.HttpGetPlayerCoinsData(FB_Handler.instance.SavedEmail, (string coins) =>
        {
            MainMenu.UserCurrentChips = int.Parse(coins);
            Debug.Log(FB_Handler.instance.SavedEmail + coins);

            mainmenuscript.SetChipsText();
            loadingscript.SetDisable();

        });
        //TO GET COINS-------------------------------------------------------------------


    }

    public void getcoins(int coin)
    {
        coins= coin;
    }

    public void buycoins(int amount)
    {
        WebRequestManager.HttpBuyCoin(amount, FB_Handler.instance.SavedUsername, coins, FB_Handler.instance.SavedEmail, () =>
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
