using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameNameSpace {
    public class Player : MonoBehaviour
    {
        public int PLayerDefaultNumber;
        public new string name;
        public string email;
        public Sprite playerSprite;
        public int coin;
        public float XP;
        public List<CardsManager.Card> cardList = new List<CardsManager.Card>();

        //------------------------------------------------

        public bool Ordered;
        public int NoOfSameSuit;
        public int NoOfSameRank;

        // These Layer and Sub-Layer are Relative Ranking among Diffrent Player.

        public int DeckLayer; // 1-6 MAIN LAYER.
        public int DeckSubLayer; // THOUSANDS OF DIFFRENT POSSIBLITIES. */

        //------------------------------------------------------------------
        // GUI

        public Text nameText;
        public Text chipsText;
        public Text xpText;
        public Image profileImage;
        public Image SelectionUI;
        public Image[] cardsImage;

        public void PopulateData()
        {
            nameText.text = name;
            chipsText.text = "Chips : " + coin.ToString();
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

        public void RefreshData()
        {
            chipsText.text = "Chips : " + coin.ToString();
            xpText.text = "XP : " + XP.ToString();
        }
    }
}

