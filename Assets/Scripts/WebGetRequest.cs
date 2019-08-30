using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetRequest : MonoBehaviour
{
    public string URL;

    private void Awake()
    {
        URL = "http://factoryprice.co.in/best_deals/point_dec.php?";
    }

    [System.Serializable]
    public class LoginMessage
    {
        public string email;
        public int coins;
        public float XP;

        public override string ToString()
        {
            return "email=" + email.ToString() + "&"
                + "points=" + coins.ToString();
        }
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string responseMessage;
        public int statusCode;
        public string status;
    }

    private void Start()
    {
        OnLogin();
    }


    public void OnLogin()
    {
        LoginMessage loginMessage = new LoginMessage { email = "abc@xyz.com", coins = 100 };

        HttpRequest<LoginMessage> httpRequest = new HttpRequest<LoginMessage>(loginMessage, URL);

        HttpHandler.Get(httpRequest, (LoginResponse loginResponse) =>
         {
             Debug.Log("Response: " + loginResponse.status);
         }, () =>   
         {
             Debug.Log("False");
         });
    }

}
