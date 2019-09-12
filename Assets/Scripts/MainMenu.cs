using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public static int currentcoin;

     public GameObject ShopPanel;
     public GameObject SettingPanel;
     public GameObject mainmenu;
     public GameObject NotEnoughCoinPanel;
     public GameObject redeempanel;
     public GameObject buychippanel;
     public GameObject buychipblack;
     public GameObject redeemblue;
     public GameObject TableAmount;

    public TextMeshProUGUI amounttext;
    public TextMeshProUGUI cointext;
    public Text cointextinfo;
    public TextMeshProUGUI date;
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
        if (amounttable < 190)
        {
            amounttable = amounttable + 20;
            amounttext.text = amounttable.ToString();
        }
        else if(amounttable == 190)
        {
            amounttable = amounttable + 10;
            amounttext.text = amounttable.ToString();
        }
        else if (amounttable >= 200 && amounttable < 500)
        {
            amounttable = amounttable + 50;
            amounttext.text = amounttable.ToString();
        }
    }


    public void minusamount()
    {
        if(amounttable > 20)
        {
            amounttable = amounttable - 20;
            amounttext.text = amounttable.ToString();
        }
    }

    public void SetAmount()
    {
        GameInstance.new_instance.MinimumBettingValue = amounttable;
        if((amounttable*300)<= currentcoin)
        {
            PlayGame();
        }
        else
        {
            //Can not Play.
            TableAmount.SetActive(false);
            NotEnoughCoinPanel.SetActive(true);
        }
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


    private void Update()
    {
        cointext.text = cointextinfo.text = currentcoin.ToString();
    }
}