using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
     public Image photo;
    public TextMeshProUGUI name;
    public List<cardsLogic.Card> cards;

  
    private void Start()
    {
        photo.sprite = FB_Handler.instance.FB_Profile.sprite;
        name.text = FB_Handler.instance.FB_UserName.text;
    }

   public  void Blind()
    {
        Debug.Log("Player " + PlayerCreater.indexInPlay + " Plays Blind");
    }

    public void Show()
    {
        Debug.Log("Player " + PlayerCreater.indexInPlay + " Plays Show");
    }

    public void Fold()
    {
        Debug.Log("Player " + PlayerCreater.indexInPlay + " Plays Fold ");

    }


 

}
