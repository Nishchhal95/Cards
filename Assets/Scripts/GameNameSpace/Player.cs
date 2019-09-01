using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameNameSpace {
    public class Player : MonoBehaviour
    {
        public string playerName;
        public Sprite playerSprite;
        public int chips;
        public float XP;
        public List<CardsManager.Card> cardList = new List<CardsManager.Card>();

 
        public Text nameText;
        public Text chipsText;
        public Text xpText;
        public Image profileImage;
        public Image SelectionUI;
        public Image[] cardsImage;

        public void PopulateData()
        {
            nameText.text = playerName;
            chipsText.text = "Chips : " + chips.ToString();
            xpText.text = "XP : " + XP.ToString();
            profileImage.sprite = playerSprite;
        }

        public void PopulateCards()
        {
            for(int i=0; i<3; i++)
            {
                cardsImage[i].sprite = cardList[i].CardSprite;
            }
        }
    }
}

