using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class coinDeductionAPI : MonoBehaviour
{

    string userEmail= FB_Handler.instance.SavedEmail;

    int coinsToBeDeducted=0;

    string Deductedcoin;




    void Start()
    {
        StartCoroutine(Upload());

        Deductedcoin = coinsToBeDeducted.ToString();
    }






    IEnumerator Upload()
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
