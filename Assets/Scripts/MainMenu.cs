﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{
     public static int UserCurrentChips;
     public static int UserCurrentDiamonds; 
     public static int LargestPotWin;
     public static int HighestChipsEver;
     public static string BestHandString;

     public GameObject ShopPanel;
     public GameObject SettingPanel;
     public GameObject mainmenu;
     public GameObject NotEnoughCoinPanel;
     public GameObject redeempanel;
     public GameObject buychippanel;
     public GameObject buychipblack;
     public GameObject redeemblue;
     public GameObject TableAmount;

    public TextMeshProUGUI BetTableText;  //Text on Betting Table Choosen Bet Text.
    public TextMeshProUGUI MainMenuChipsText;  //Text on Main Menu.
    public TextMeshProUGUI MainMenuDiamondsText;  //Text on Main Menu.

    public Text PlayerInfoChipsText;
    public Text PlayerInfoDiamondsText;
    public TextMeshProUGUI DateText;

    public TextMeshProUGUI LargestPotText;
    public TextMeshProUGUI HighestChipsText;
    public TextMeshProUGUI BestHandChipsText;

    int BetAmount;

    private void Awake()
    {
        Utils_X.DoActionAfterSecondsAsync(CheckFB, 0.1f);
        BetAmount = 10;
        BetTableText.text = BetAmount.ToString();
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("Date", "0") == "0")
        {
            //If Date is NULL , Save Current Time and Show it.
            PlayerPrefs.SetString("Date", System.DateTime.Now.ToString("dd-MM-yyyy"));
        }

        DateText.text = ": " + PlayerPrefs.GetString("Date");

        if (PlayerPrefs.GetInt("LargestPot", 0) == 0)
        { 
            LargestPotText.text = ": ------------";
        }
        else
        {
            LargestPotText.text = ": " + PlayerPrefs.GetInt("LargestPot").ToString() + "Chips";
        }
       


        if (PlayerPrefs.GetInt("HighestChips", 0) == 0)
        {
            HighestChipsText.text = ": ------------";
        }
        else
        {
            HighestChipsText.text = ": " + PlayerPrefs.GetInt("HighestChips").ToString() + "Chips";
        }

        if (PlayerPrefs.GetString("BestHand", null) == null)
        {
            BestHandChipsText.text = ": ------------";
        }
        else
        {
            BestHandChipsText.text = ": " + PlayerPrefs.GetString("BestHand");
        }

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
        if (BetAmount < 190)
        {
            BetAmount = BetAmount + 20;
        }
        else if(BetAmount == 190)
        {
            BetAmount = BetAmount + 10;
        }
        else if (BetAmount >= 200 && BetAmount < 500)
        {
            BetAmount = BetAmount + 50;
        }

        BetTableText.text = BetAmount.ToString();

    }


    public void minusamount()
    {
        if(BetAmount > 200)
        {
            BetAmount = BetAmount - 50;
        }
        else if(BetAmount == 200)
        {
            BetAmount = BetAmount - 10;
        }
        else if(BetAmount < 200 && BetAmount > 10)
        {
            BetAmount = BetAmount - 20;
        }

        BetTableText.text = BetAmount.ToString();
    }

    public void SetAmount()
    {
        GameInstance.new_instance.MinimumBettingValue = BetAmount;
        if((BetAmount * 300)<= UserCurrentChips)
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
        //buychipblack.SetActive(true);
        redeempanel.SetActive(true);
        //buychippanel.SetActive(false);
        redeemblue.SetActive(true);
    }
    public void showbuychip()
    {
        //buychipblack.SetActive(false);
        redeempanel.SetActive(false);
        //buychippanel.SetActive(true);
        redeemblue.SetActive(false);
    }

    

    public void SetChipsText()
    {
        PlayerInfoChipsText.text = MainMenuChipsText.text = UserCurrentChips.ToString();
    }

    public void SetDiamondsText()
    {
        PlayerInfoDiamondsText.text = MainMenuDiamondsText.text = UserCurrentDiamonds.ToString();
    }

}