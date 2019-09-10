using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emailGenrator : MonoBehaviour
{

    private string email="support@jungleeteenpatti.com";

    private string subject;

    private string body = "<< How can we help you? >>";


    public void SendEmailButton()
    {
        if(FB_Handler.instance.SavedUsername == null)
        {
            subject = "Support Request";
        }
        else
        {
            subject = "Support Request from " + FB_Handler.instance.SavedUsername.ToString();
        }
        SendEmail(email,subject,body);
    }

    private void SendEmail(string toEmail, string emailSubject, string emailBody)
    {
        emailSubject = System.Uri.EscapeUriString(emailSubject);
        emailBody = System.Uri.EscapeUriString(emailBody);
        Application.OpenURL("mailto:" + toEmail + "?subject=" + emailSubject + "&body=" + emailBody);
    }


}
