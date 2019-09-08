using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GameNameSpace;


public class SENDDATAwebReq : MonoBehaviour
{

    //string _name = FB_Handler.instance.SavedUsername;

    //string _email = FB_Handler.instance.SavedEmail;

    //Sprite _profile =FB_Handler.instance.SavedProfile;

    fetchDynamicCoinsApi f;
    _GameManager g;

    string _number=  "" ;



    string dynamiCoinVal;
    string imei = "iemei";

    string minimumbet;



    void Start()
    {
        StartCoroutine(Upload());
        dynamiCoinVal = f.coinsFetched;   //ssend dynamic coin fetched from getcoin server and sends it.
        
        minimumbet=g.MinimumBettingValue.ToString();
    }





    //on login data send.
    IEnumerator Upload()
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
}