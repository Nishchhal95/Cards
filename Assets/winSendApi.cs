using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GameNameSpace;

public class winSendApi : MonoBehaviour
{

    string _email = FB_Handler.instance.SavedEmail;

    string winningAmount;


    _GameManager g;




    //send data on winning event ...........
    void Start()
    {
        StartCoroutine(Upload());

        winningAmount = g.TotalPot.ToString();
    }

    IEnumerator Upload()
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
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}

