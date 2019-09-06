using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SENDDATAwebReq : MonoBehaviour
{

    string _name = FB_Handler.instance.SavedUsername;

    string _email = FB_Handler.instance.SavedEmail;

    Sprite _profile =FB_Handler.instance.SavedProfile;

    




    void Start()
    {
        StartCoroutine(Upload());

    
    }






    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();



        form.AddField("NameField", _name);

        form.AddField("EmailField", _email);

        form.AddField("ProfilePicture", _profile.ToString());

        form.AddField("Coins", "5000 Bonus");

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