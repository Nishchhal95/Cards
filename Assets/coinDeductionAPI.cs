using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GameNameSpace;

public class coinDeductionAPI : MonoBehaviour
{

    string userEmail= FB_Handler.instance.SavedEmail;

    

    string Deductedcoin;


    Player p;

    void Start()
    {
        StartCoroutine(UploadCoinsDeducted());

        Deductedcoin = p.coin.ToString();   //takes remain coin from player script and send to server
    }






    IEnumerator UploadCoinsDeducted()
    {
        WWWForm form = new WWWForm();

        

        form.AddField("email of user ", userEmail);
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
