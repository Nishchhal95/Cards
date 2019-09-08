using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GameNameSpace;
using UnityEngine.UI;


public class SENDDATAwebReq : MonoBehaviour
{
    public static SENDDATAwebReq Instance = null;

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



    public Text messageText;
    //  string message = "successfully sent data to server";


    // ----------------------------------variables---------------
    string _name = FB_Handler.instance.SavedUsername;

    string _email = FB_Handler.instance.SavedEmail;

    Sprite _profile = FB_Handler.instance.SavedProfile;

    jsonPluginWEBREQ j;
    _GameManager g;

    Player p;

    string _number = "";



    string dynamiCoinVal;
    string imei = "iemei";

    string minimumbet;


    string winningAmount;

    string Deductedcoin;







    void Start()
    {
        StartCoroutine(UploadLoginData());

        StartCoroutine(UploadWinDATA());

        StartCoroutine(UploadCoinsDeducted());

        //------------------


        dynamiCoinVal = j.coinsFetched;   //ssend dynamic coin fetched from getcoin server and sends it.

        minimumbet = g.MinimumBettingValue.ToString();



        winningAmount = g.TotalPot.ToString();




        Deductedcoin = p.coin.ToString();   //takes remain coin from player script and send to server
        messageText = GetComponent<Text>();
    }





    //on login data send.
    IEnumerator UploadLoginData()
    {
        WWWForm form = new WWWForm();



        //form.AddField("NameField", _name  ) ;

        //form.AddField("EmailField", _email);

        //form.AddField("ProfilePicture", _profile.ToString());

        form.AddField("Coins", dynamiCoinVal);   //sends dynamic fetched value from jsonpluginwebReq to server.
        form.AddField("numbersFeild", _number);
        form.AddField("IMEI feild", imei);
        form.AddField("MinimumBet", minimumbet);     //sending minimum bet



        using (UnityWebRequest www = UnityWebRequest.Post("Languagelive.xyz/casino/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);

            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


    //--------upload win data

    IEnumerator UploadWinDATA()
    {
        WWWForm form = new WWWForm();




        form.AddField("EmailField", _email);
        form.AddField("winningAmount", winningAmount);




        using (UnityWebRequest www = UnityWebRequest.Post("Languagelive.xyz/casino/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                messageText.text = "netwroking error";
            }
            else
            {
                // messageText.text = message;
                Debug.Log("Form login upload complete!");
            }
        }
    }



    //--------upload deducted coins---------------

    IEnumerator UploadCoinsDeducted()
    {
        WWWForm form = new WWWForm();



        form.AddField("email of user ", _email);
        form.AddField("Coins deducted ", Deductedcoin);



        using (UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

}