using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class cardsLogic : MonoBehaviour
{
    public enum SuitEnum { Hearts = 1, Clubs = 2, Diamonds = 3, Spades = 4 }

    public List<Card> CardDeck = new List<Card>();  // List of Randomly Ordered 52 Cards.

   // public List<Card> DiscardDeck; // List of Cards which has been already used.

    public int playerNo;     // no of people to assign 3 cards to them.
    int k;  // that will randomly fetch cards from database

    public class Card   //A Card class we are going to use in Lists. A datatype.
    {     
        public SuitEnum Suit;
        public int Rank;
        public Card(SuitEnum newSuit, int newRank)
        {
            Suit = newSuit;
            Rank = newRank;
        }
    }

    void Start()
    {
        MakeDatabase();
       // AssignCardToXpeople(); 
    }

    public void MakeDatabase()
    {
        // Here we have to Add Random Cards to List. 
        Card CardToAdd = new Card(SuitEnum.Hearts, 1);      // "A" of Hearts.
        CardDeck.Insert(0, CardToAdd);                      //Add it.

        Debug.Log(CardToAdd.Rank + " of " + CardToAdd.Suit); 
    }

    public void AssignCardToXpeople()
    {
        Debug.Log("Assigning three random cards to" + playerNo + " number of players : ");
        for (int i = 1; i <= playerNo; i++)
        {
            Debug.Log("assigning 3 cards to player number  " + i); // so that its known which card assigned to which player
            AssignCards();
        }
    }
    public void AssignCards()
    {   

        //Here have to remove top card from deck.
        //and add to discardedcard list
        //and distribute this card to player.
        //  WE HAVE TO CHANGE BELOW CODE.

        // printing  3 random cards for a player
        for (int i=0;i<3;i++)
        {
            k = Random.Range(0, CardDeck.Count);  // get random index from list

            Debug.Log(CardDeck[k]);    //fetching value from cards database

            CardDeck.RemoveAt(k);  // avoid repeattion of aassignment of cards
        }
    }



 
}
