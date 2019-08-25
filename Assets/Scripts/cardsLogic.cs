using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardsLogic : MonoBehaviour
{
    public enum SuitEnum { Hearts = 1, Clubs = 2, Diamonds = 3, Spades = 4 }

    
    public List<Card> CardDeck = new List<Card>();  // List of Ordered 52 Cards.

    public List<CardList> PlayersList = new List<CardList>();  //List of Players

    public int playerNo;     // No of people to assign 3 cards to them.
    int k;  // That will randomly fetch cards from database

    [System.Serializable]
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

    [System.Serializable]
    public class CardList   
    {
        public List<Card> CardsList = new List<Card>();  //List of Card of specific player.
    }

    void Start()
    {
        MakeDatabase();
    }

    public void MakeDatabase()
    {
        //Create Ordered List of Cards.

        for(int i=1; i<=4; i++)  //Loop for Suits
        {    
            for(int j=1; j<=13; j++)  //Loop for Number.
            {
                Card CardToAdd = new Card((SuitEnum)i, j);      // "A" of Hearts.
                CardDeck.Insert(0, CardToAdd); //Add it.

                // Debug.Log(CardToAdd.Rank + " of " + CardToAdd.Suit);
            }
        }

        AssignCardToXpeople();
    }

    public void AssignCardToXpeople()
    {
        Debug.Log("Assigning three random cards to " + playerNo + " number of players : ");

        for (int i = 1; i <= playerNo; i++)
        {
            Debug.Log("Assigning 3 cards to Player Number " + i); // so that its known which card assigned to which player
            AssignCards();
          
        }
    }
    public void AssignCards()
    {
       //Assign Random Card to Player from List and Removes that Card from List.

       // Here we will create new list of player

        for (int i=0;i<3;i++)
        {
            k = Random.Range(0, CardDeck.Count);  // Get random index from cards list.

            Debug.Log(CardDeck[k].Rank + "of" + CardDeck[k].Suit);    // Fetching value from cards database.

            CardDeck.RemoveAt(k);  // Avoid repeattion of aassignment of cards. 


            //Here we will add Cards to that new list of Player.
        }

    }

}
