using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedeemManager : MonoBehaviour
{
    public GameObject Redeempanel;
    public TextMeshProUGUI chiptext;
    public long ChipofPlayer;
    public TMP_InputField Name;
    public TMP_InputField Paytm;
    public TMP_InputField Email;
    public TMP_InputField Redeemamounttext;
    int redeemamount;
    int redeemchip;

    private void Update()
    {
        ChipofPlayer = long.Parse(chiptext.text);
    }
    private void showstats()
    {
     
        Name.text = FB_Handler.instance.FB_UserName.text;
        Email.text = FB_Handler.instance.FB_Email.text;
   
    }


    public void amountredeem( int amount)
    {
        redeemamount = amount;
        Redeemamounttext.text = redeemamount.ToString();
    }
    
    public void chipredeem( int chip)
    {
       
        redeemchip = chip;
        if (checkeligible())
        {
            Redeempanel.SetActive(true);
            showstats();
        }
        else
        {
            Debug.Log("less chip");
        }

    }


    public bool checkeligible()
    {
        if(ChipofPlayer >= redeemchip)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
