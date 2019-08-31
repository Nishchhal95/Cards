using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Image PlayerProfile;
    public TextMeshProUGUI PlayerName;
    public List<CardsManager.Card> cards;
    public bool blind = false;
    public bool show = false;
    public bool fold = false;

    private void Start()
    {
        //PlayerProfile.sprite = FB_Handler.instance.FB_Profile.sprite;
       // PlayerName.text = FB_Handler.instance.FB_UserName.text;
    }

   public  void Blind()
    {
        blind = true;
        Debug.Log("Player " + PlayerCreater.indexInPlay + " Plays Blind");
    }

    public void Show()
    {
        show = true;
        Debug.Log("Player " + PlayerCreater.indexInPlay + " Plays Show");
    }

    public void Fold()
    {
        fold = true;
        Debug.Log("Player " + PlayerCreater.indexInPlay + " Plays Fold ");

    }


 

}
