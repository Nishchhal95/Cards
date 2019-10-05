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
    private string imei;

    private void Awake()
    {
        //Utils_X.DoActionAfterSecondsAsync(SetData, 0.1f);
        imei = SystemInfo.deviceUniqueIdentifier;
    }

    void SetData()
    {
        FB_UserName.text = FB_Handler.instance.SavedUsername;
        FB_UserName2.text = FB_Handler.instance.SavedUsername;
        FB_Email.text = FB_Handler.instance.SavedEmail;
        FB_Profile.sprite = FB_Handler.instance.SavedProfile;
        FB_Profile2.sprite = FB_Handler.instance.SavedProfile;
        jsonPluginWEBREQ.Instance.initData(imei);
    }

}
