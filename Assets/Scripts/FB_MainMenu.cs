using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FB_MainMenu : MonoBehaviour
{
    public Text FB_UserName;
    public Text FB_UserName2;
    public Text FB_Email;
    public Image FB_Profile;
    public Image FB_Profile2;

    private void Awake()
    {
        Utils.DoActionAfterSecondsAsync(SetData, 0.1f);
    }

    void SetData()
    {
        FB_UserName.text = FB_Handler.instance.SavedUsername;
        FB_UserName2.text = FB_Handler.instance.SavedUsername;
        FB_Email.text = FB_Handler.instance.SavedEmail;
        FB_Profile.sprite = FB_Handler.instance.SavedProfile;
        FB_Profile2.sprite = FB_Handler.instance.SavedProfile;
    }

}
