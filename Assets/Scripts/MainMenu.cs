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
     public GameObject TableAmount;

    public TextMeshProUGUI amounttext;
    public TextMeshProUGUI date;
    public TMP_InputField share;
    int amounttable;

    private void Awake()
    {
        Utils.DoActionAfterSecondsAsync(CheckFB, 0.1f);
        amounttable = 10;
        amounttext.text = amounttable.ToString();
    }

    private void Start()
    {
        if(PlayerPrefs.GetString("date","0") == "0")
        {
            PlayerPrefs.SetString("date", System.DateTime.Now.ToString("MM/dd/yyyy"));
        }
        date.text = PlayerPrefs.GetString("date");
        share.text = "www.facebook.com/CasinoCards";
    }

    void CheckFB()
    {
        if (FB_Handler.instance.CheckIfLoggedIn() == true)
        {
            mainmenu.SetActive(true);
        }
    }


    public void tableamountpanel()
    {
        TableAmount.SetActive(true);
    }

    public void plusamount()
    {
        if (amounttable < 200)
        {
            amounttable = amounttable + 10;
            amounttext.text = amounttable.ToString();
        }
    }


    public void minusamount()
    {
        if(amounttable>10)
        {
            amounttable = amounttable - 10;
            amounttext.text = amounttable.ToString();
        }
    }

    public void SetAmount()
    {
        GameInstance.new_instance.MinimumBettingValue = amounttable;
    }

    public void PlayGame()
    {
        TableAmount.SetActive(false);
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
        mainmenu.SetActive(false);
    }

    public void SettingPanelhide()
    {
        SettingPanel.SetActive(false);
        mainmenu.SetActive(true);
    }

    public void howtoplayshow()
    {
        SceneManager.LoadScene("howToPlay");
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