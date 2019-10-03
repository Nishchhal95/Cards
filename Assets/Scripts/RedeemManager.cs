using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedeemManager : MonoBehaviour
{
    public GameObject Redeempanel;
    public GameObject lesspanel;
    public TextMeshProUGUI chiptext;
    public long ChipofPlayer;
    public TMP_InputField Name;
    public TMP_InputField Paytm;
    public TMP_InputField Email;
    public TMP_InputField Redeemamounttext;
    int redeemamount;
    int redeemdiamonds;
    long MobieNumber;
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
    
    public void diamondreedeem( int diamonds)
    {
       
        redeemdiamonds = diamonds;
        if (checkeligible())
        {
            Redeempanel.SetActive(true);
            showstats();
        }
        else
        {
            lesspanel.SetActive(true);
        }

    }


    public bool checkeligible()
    {
        if(ChipofPlayer >= redeemdiamonds)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void redeemchipApi()
    {
        /*MobieNumber = long.Parse(Paytm.text);
        WebRequestManager.Httpredeem(redeemamount, FB_Handler.instance.FB_Email.text, redeemchip, MobieNumber, () =>
        {
              Debug.Log("success redeem");
              Redeempanel.SetActive(false);
        });
        */

        MainMenu.UserCurrentDiamonds += redeemdiamonds;
        Redeempanel.SetActive(false);
    }


}
