using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emailGenrator : MonoBehaviour
{
    // Start is called before the first frame update
    public string email="support@jungleeteenpatti.com";


    public string subject= "support request from userName= Guest858886 user id= 675005994";



    public string body = "<<HOW an we help you?>>                              "    +
        "              ======DO NOT MODIFY BELOW====            "                    +

        " player id: 67500944      device id: dc206768677868      game id : no game in proggress" +
      "============================";






    public void SendEmailBtn()
    {

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string URL)
    {
        return WWW.EscapeURL(URL).Replace("+", "%20");
    }

}
