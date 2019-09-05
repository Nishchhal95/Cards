using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNameSpace;

public class CardsManager : MonoBehaviour
{
    public static CardsManager instance = null;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public enum SuitEnum { Hearts = 1, Clubs = 2, Diamonds = 3, Spades = 4 }
    public enum ColorEnum { Red = 1, Black = 2 }

    public List<Card> CardDeck = new List<Card>();  // List of Ordered 52 Cards.

    //public List<GameObject> TopRankers = new List<GameObject>();  // List of Top Player after Comparision.

    int NumberOfPlayers;     // No of people to assign 3 cards to them.
   // int k;  // That will randomly fetch cards from database

    [System.Serializable]
    public class Card   //A Card class we are going to use in Lists. A datatype.
    {
        public SuitEnum Suit;
        public ColorEnum Color;
        public int Rank;
        public Sprite CardSprite;
        public Card(SuitEnum newSuit, int newRank, ColorEnum newColor, Sprite newSprite)
        {
            Suit = newSuit;
            Rank = newRank;
            Color = newColor;
            CardSprite = newSprite;
        }
    }

    // public List<CardList> P_List = new List<CardList>();  //List of Card of specific player.

    // [System.Serializable]
    // public class CardList
    // {
    // public List<Card> GetComponent<Player>().cardList = new List<Card>();  //List of Card of specific player.
    //   public int PlayerNo;

    //public bool Ordered;
    // public int NoOfSameSuit;
    //public int NoOfSameRank;

    // These Layer and Sub-Layer are Relative Ranking among Diffrent Player.

    //public int DeckLayer; // 1-6 MAIN LAYER.
    // public int DeckSubLayer; // THOUSANDS OF DIFFRENT POSSIBLITIES.

    // }


    public void MakeDatabase()
    {
        NumberOfPlayers = _GameManager.numberOfPlayer;

        //Create Ordered List of Cards.

        for (int i = 1; i <= 4; i++)  //Loop for Suits
        {
            for (int j = 1; j <= 13; j++)  //Loop for Number.
            {
                string temp = "Sprites/Cards/" + (SuitEnum)i + "/" + j; //Create Temporary String Dont Change Path. In code as well as Files Directory.

                if (i == 1 || i == 3)  //For Color
                {
                    Card CardToAdd = new Card((SuitEnum)i, j, (ColorEnum)1, Resources.Load<Sprite>(temp));      // "A" of Hearts.
                    CardDeck.Insert(0, CardToAdd); //Add it.
                }
                else
                {
                    Card CardToAdd = new Card((SuitEnum)i, j, (ColorEnum)2, Resources.Load<Sprite>(temp));      // "A" of Hearts.
                    CardDeck.Insert(0, CardToAdd); //Add it.
                }
            }
        }

    }


    public List<Card> Get3Cards()
    {
        List<Card> ThreeCards = new List<Card>();

        for(int i=0; i<3; i++)
        {
            int k = Random.Range(0, CardDeck.Count);  // Get random index from cards list.
            string temp = "Sprites/Cards/" + CardDeck[k].Suit + "/" + CardDeck[k].Rank; //Create Temporary String Dont Change Path. In code as well as Files Directory.
            ThreeCards.Add(new Card(CardDeck[k].Suit, CardDeck[k].Rank, CardDeck[k].Color, Resources.Load<Sprite>(temp)));
            CardDeck.RemoveAt(k);  // Avoid repeattion of aassignment of cards.
        }
        return ThreeCards;
    }

 

}
