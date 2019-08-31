using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject ShopPanel;
    public GameObject SettingPanel;
    public GameObject mainmenu;
     public GameObject howtoplaypanel;
     public GameObject redeempanel;
     public GameObject buychippanel;
     public GameObject buychipblack;
     public GameObject redeemblue;
    public Image mainphoto;
    public TextMeshProUGUI playername;
    public Text email;

    private void Update()
    {
        mainphoto.sprite = FB_Handler.instance.FB_Profile.sprite;
        playername.text = FB_Handler.instance.FB_UserName.text;
       // email.text = FB_Handler.instance.FB_Email.text;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Player");
    }

    public void Shop()
    {
       
        ShopPanel.SetActive(true);
        mainmenu.SetActive(false);
    }

    public void Shophide()
    {
        ShopPanel.SetActive(false);
        mainmenu.SetActive(true);
    }

    public void SettingPanelshow()
    {
        SettingPanel.SetActive(true);
    }

    public void howtoplayshow()
    {
        howtoplaypanel.SetActive(true);
        mainmenu.SetActive(false);
    }

    public void howtoplayhide()
    {
        howtoplaypanel.SetActive(false);
        mainmenu.SetActive(true);
    }

    public void showredeem()
    {
        buychipblack.SetActive(true);
        redeempanel.SetActive(true);
        buychippanel.SetActive(false);
        redeemblue.SetActive(true);
    }
    public void showbuychip()
    {
        buychipblack.SetActive(false);
        redeempanel.SetActive(false);
        buychippanel.SetActive(true);
        redeemblue.SetActive(false);
    }

}