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

        public List<Card> cardList = new List<Card>();

        public Text nameText;
        public Text chipsText;
        public Text xpText;

        public void PopulateData()
        {
            nameText.text = playerName;
            chipsText.text = chips.ToString();
            xpText.text = XP.ToString();
        }
    }
}

