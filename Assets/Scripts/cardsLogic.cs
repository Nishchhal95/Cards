using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardsLogic : MonoBehaviour
{
    //---------------------------------------------------------------------------------
    // WARNING : DONT CHANGE THE ORDER IN WHICH SCRIPT LINES ARE WRITTEN. THEY MATTERS.
    //---------------------------------------------------------------------------------
    public enum SuitEnum { Hearts = 1, Clubs = 2, Diamonds = 3, Spades = 4 }
    public enum ColorEnum { Red = 1, Black = 2 }

    public List<Card> CardDeck = new List<Card>();  // List of Ordered 52 Cards.

    public int NumberOfPlayers;     // No of people to assign 3 cards to them.
    int k;  // That will randomly fetch cards from database

    [System.Serializable]
    public class Card   //A Card class we are going to use in Lists. A datatype.
    {
        public SuitEnum Suit;
        public ColorEnum Color;
        public int Rank;
        public Card(SuitEnum newSuit, int newRank, ColorEnum newColor)
        {
            Suit = newSuit;
            Rank = newRank;
            Color = newColor;
        }
    }

    public List<CardList> PlayersList = new List<CardList>();  //List of Card of specific player.

    [System.Serializable]
    public class CardList
    {
        public List<Card> CardsList = new List<Card>();  //List of Card of specific player.
        public int PlayerNo;

        public bool Ordered;
        public int NoOfSameSuit;
        public int NoOfSameRank;

    }

    void Start()
    {
        MakeDatabase();
    }

    public void MakeDatabase()
    {
        //Create Ordered List of Cards.

        for (int i = 1; i <= 4; i++)  //Loop for Suits
        {
            for (int j = 1; j <= 13; j++)  //Loop for Number.
            {
                if (i == 1 || i == 3)  //For Color
                {
                    Card CardToAdd = new Card((SuitEnum)i, j, (ColorEnum)1);      // "A" of Hearts.
                    CardDeck.Insert(0, CardToAdd); //Add it.
                }
                else
                {
                    Card CardToAdd = new Card((SuitEnum)i, j, (ColorEnum)2);      // "A" of Hearts.
                    CardDeck.Insert(0, CardToAdd); //Add it.
                }
            }
        }
        AssignCardToXpeople();
    }

    public void AssignCardToXpeople()
    {
        Debug.Log("Assigning three random cards to " + NumberOfPlayers + " number of players : ");

        for (int i = 1; i <= NumberOfPlayers; i++)
        {
            Debug.Log("Assigning 3 cards to Player Number " + i); // so that its known which card assigned to which player
            PlayersList.Add(new CardList() { PlayerNo = i });  //Add Player which Contains Cards and Player Number.
            AssignCards(i);
            CheckSequence(i);
            CheckSuit(i);
            CheckRank(i);
        }
    }
    public void AssignCards(int playerno)
    {
        //Assign Random Card to Player from List and Removes that Card from List.

        for (int i = 0; i < 3; i++)
        {
            k = Random.Range(0, CardDeck.Count);  // Get random index from cards list.

            Debug.Log(CardDeck[k].Rank + "of" + CardDeck[k].Suit);    // Fetching value from cards database.

            PlayersList[playerno - 1].CardsList.Add(new Card(CardDeck[k].Suit, CardDeck[k].Rank, CardDeck[k].Color));  //Add Card to respective Player.

            CardDeck.RemoveAt(k);  // Avoid repeattion of aassignment of cards. 

        }

    }

    //---------------------------------


    public void CheckRank(int ForWhichPlayer)  //Tells Behaviour of Deck with respect to "RANK".
    {
        if (PlayersList[ForWhichPlayer - 1].CardsList[0].Rank == PlayersList[ForWhichPlayer - 1].CardsList[1].Rank)
        {
            if (PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == PlayersList[ForWhichPlayer - 1].CardsList[2].Rank)
            {
                //All have Same Rank.  // ALL 3 ARE SAME.
                PlayersList[ForWhichPlayer - 1].NoOfSameRank = 3;
            }
            else
            {
                //Two Cards have Same Rank.  // FIRST 2 ARE SAME.  
                PlayersList[ForWhichPlayer - 1].NoOfSameRank = 2;
            }
        }
        else
        {
            if (PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == PlayersList[ForWhichPlayer - 1].CardsList[2].Rank)
            {
                //Two Cards have Same Rank.   // LAST 2 ARE SAME.
                PlayersList[ForWhichPlayer - 1].NoOfSameRank = 2;
            }
            else
            {
                //No Card have Same Rank.   // NONE ARE SAME.
                PlayersList[ForWhichPlayer - 1].NoOfSameRank = 0;
            }
        }
    }

    public void CheckSuit(int ForWhichPlayer)  //Tells Behaviour of Deck with respect to "Suit".
    {
        if (PlayersList[ForWhichPlayer - 1].CardsList[0].Suit == PlayersList[ForWhichPlayer - 1].CardsList[1].Suit)
        {
            if (PlayersList[ForWhichPlayer - 1].CardsList[1].Suit == PlayersList[ForWhichPlayer - 1].CardsList[2].Suit)
            {
                //All have Same Suit.  // ALL 3 ARE SAME.
                PlayersList[ForWhichPlayer - 1].NoOfSameSuit = 3;
            }
            else
            {
                //Two Cards have Same Suit.  // FIRST 2 ARE SAME.  
                PlayersList[ForWhichPlayer - 1].NoOfSameSuit = 2;
            }
        }
        else
        {
            if (PlayersList[ForWhichPlayer - 1].CardsList[1].Suit == PlayersList[ForWhichPlayer - 1].CardsList[2].Suit)
            {
                //Two Cards have Same Suit.   // LAST 2 ARE SAME.
                PlayersList[ForWhichPlayer - 1].NoOfSameSuit = 2;
            }
            else
            {
                //No Card have Same Suit.   // NONE ARE SAME.
                PlayersList[ForWhichPlayer - 1].NoOfSameSuit = 0;
            }
        }
    }

    public void SortInDesending(int ForWhichPlayer)
    {
        if (PlayersList[ForWhichPlayer - 1].CardsList[0].Rank <= PlayersList[ForWhichPlayer - 1].CardsList[1].Rank)
        {
            int temp = PlayersList[ForWhichPlayer - 1].CardsList[0].Rank;
            PlayersList[ForWhichPlayer - 1].CardsList[0].Rank = PlayersList[ForWhichPlayer - 1].CardsList[1].Rank;
            PlayersList[ForWhichPlayer - 1].CardsList[1].Rank = temp;
        }

        if (PlayersList[ForWhichPlayer - 1].CardsList[1].Rank <= PlayersList[ForWhichPlayer - 1].CardsList[2].Rank)
        {
            int temp = PlayersList[ForWhichPlayer - 1].CardsList[1].Rank;
            PlayersList[ForWhichPlayer - 1].CardsList[1].Rank = PlayersList[ForWhichPlayer - 1].CardsList[2].Rank;
            PlayersList[ForWhichPlayer - 1].CardsList[2].Rank = temp;
        }

        if (PlayersList[ForWhichPlayer - 1].CardsList[0].Rank <= PlayersList[ForWhichPlayer - 1].CardsList[1].Rank)
        {
            int temp = PlayersList[ForWhichPlayer - 1].CardsList[0].Rank;
            PlayersList[ForWhichPlayer - 1].CardsList[0].Rank = PlayersList[ForWhichPlayer - 1].CardsList[1].Rank;
            PlayersList[ForWhichPlayer - 1].CardsList[1].Rank = temp;
        }
    }

    public void CheckSequence(int ForWhichPlayer) //Check Sequence of Cards in a Deck. Will return "True" if in sequence, else "False".
    {
        SortInDesending(ForWhichPlayer);  // NEED TO BE SORTED IN DESENDING ORDER.

        if ((PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == PlayersList[ForWhichPlayer - 1].CardsList[0].Rank - 1)
            && (PlayersList[ForWhichPlayer - 1].CardsList[2].Rank == PlayersList[ForWhichPlayer - 1].CardsList[1].Rank - 1))
        {
            //IN CONSECUTIVE DESENDING SEQUENCE
            PlayersList[ForWhichPlayer - 1].Ordered = true;
        }
        else
        {
            //NOT IN SEQUENCE
            PlayersList[ForWhichPlayer - 1].Ordered = false;
        }

    }

    //---------------------------------

    // method to compare 3 ranks of the cards

    public void compareHighestofThree(int a, int b, int c)
    {

        int higheshPriority = a;
        if (b >= a && b >= c)
        {
            higheshPriority = b;
        }

        else if (c >= a && c >= b)
        {
            higheshPriority = c;
        }
        else
        {
            higheshPriority = a;
        }

        // Debug.Log("card with highest priority is " + higheshPriority);
    }






    //to check rank of 3 cards index consecutively 

    void printHighestCardRank()
    {

        compareHighestofThree(CardDeck[k].Rank, CardDeck[k + 1].Rank, CardDeck[k + 2].Rank);
    }


}
