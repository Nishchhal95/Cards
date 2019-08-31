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

    public List<CardList> TopRankers = new List<CardList>();  // List of Top Player after Comparision.

    int NumberOfPlayers;     // No of people to assign 3 cards to them.
    int k;  // That will randomly fetch cards from database

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

    public List<CardList> PlayersList = new List<CardList>();  //List of Card of specific player.

    [System.Serializable]
    public class CardList
    {
        public List<Card> CardsList = new List<Card>();  //List of Card of specific player.
        public int PlayerNo;

        public bool Ordered;
        public int NoOfSameSuit;
        public int NoOfSameRank;

        // These Layer and Sub-Layer are Relative Ranking among Diffrent Player.

        public int DeckLayer; // 1-6 MAIN LAYER.
        public int DeckSubLayer; // THOUSANDS OF DIFFRENT POSSIBLITIES.

    }

    void Start()
    {
        NumberOfPlayers = _GameManager.numberOfPlayer;
    }

    public List<Card> Get3Cards()
    {
        List<Card> ThreeCards = new List<Card>();

        for(int i=0; i<3; i++)
        {
            k = Random.Range(0, CardDeck.Count);  // Get random index from cards list.
            string temp = "Sprites/Cards/" + CardDeck[k].Suit + "/" + CardDeck[k].Rank; //Create Temporary String Dont Change Path. In code as well as Files Directory.
            ThreeCards.Add(new Card(CardDeck[k].Suit, CardDeck[k].Rank, CardDeck[k].Color, Resources.Load<Sprite>(temp)));
            CardDeck.RemoveAt(k);  // Avoid repeattion of aassignment of cards.
        }

        return ThreeCards;
    }

    public void MakeDatabase()
    {
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
        //AssignCardToXpeople();
    }

    public void AssignCardToXpeople()  //MAIN FUNCTION
    {
        Debug.Log("Assigning three random cards to " + NumberOfPlayers + " number of players : ");

        for (int i = 1; i <= NumberOfPlayers; i++)
        {
            //Debug.Log("Assigning 3 cards to Player Number " + i); // so that its known which card assigned to which player
            PlayersList.Add(new CardList() { PlayerNo = i });  //Add Player which Contains Cards and Player Number.
            AssignCards(i);
            CheckSequence(i);
            CheckSuit(i);
            CheckRank(i);
            PlayersList[i-1].DeckLayer = AssignDeckLayer(i);  //RETURNED VALUE OF DECK LAYER.
            PlayersList[i - 1].DeckSubLayer = AssignDeckSubLayer(i, PlayersList[i - 1].DeckLayer);  //RETURNED VALUE OF DECK SUB-LAYER.
            RankPlayers(i);  //RANKING HAPPENS HERE.
        }
    }
    public void AssignCards(int playerno)
    {
        //Assign Random Card to Player from List and Removes that Card from List.

        for (int i = 0; i < 3; i++)
        {
            k = Random.Range(0, CardDeck.Count);  // Get random index from cards list.

            //  Debug.Log(CardDeck[k].Rank + "of" + CardDeck[k].Suit);    // Fetching value from cards database.

            string temp = "Sprites/Cards/" + CardDeck[k].Suit + "/" + CardDeck[k].Rank; //Create Temporary String Dont Change Path. In code as well as Files Directory.

            PlayersList[playerno - 1].CardsList.Add(new Card(CardDeck[k].Suit, CardDeck[k].Rank, CardDeck[k].Color, Resources.Load<Sprite>(temp) ));  //Add Card to respective Player.
            Debug.Log("Sprites/Cards/" + CardDeck[k].Suit + "/" + CardDeck[k].Rank);
            CardDeck.RemoveAt(k);  // Avoid repeattion of aassignment of cards.

        }

    }

    //---------------------------------


    public void CheckRank(int ForWhichPlayer)  //Tells Behaviour of Deck with respect to "RANK".
    {
        if(PlayersList[ForWhichPlayer - 1].CardsList[0].Rank == PlayersList[ForWhichPlayer - 1].CardsList[1].Rank &&
            PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == PlayersList[ForWhichPlayer - 1].CardsList[2].Rank)
        {
            PlayersList[ForWhichPlayer - 1].NoOfSameRank = 3;
        }
        else
        if (PlayersList[ForWhichPlayer - 1].CardsList[0].Rank == PlayersList[ForWhichPlayer - 1].CardsList[1].Rank ||
            PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == PlayersList[ForWhichPlayer - 1].CardsList[2].Rank ||
            PlayersList[ForWhichPlayer - 1].CardsList[2].Rank == PlayersList[ForWhichPlayer - 1].CardsList[0].Rank)
        {
            PlayersList[ForWhichPlayer - 1].NoOfSameRank = 2;
        }
        else
        {
            PlayersList[ForWhichPlayer - 1].NoOfSameRank = 0;
        }
    }

    public void CheckSuit(int ForWhichPlayer)  //Tells Behaviour of Deck with respect to "Suit".
    {
        if (PlayersList[ForWhichPlayer - 1].CardsList[0].Suit == PlayersList[ForWhichPlayer - 1].CardsList[1].Suit &&
            PlayersList[ForWhichPlayer - 1].CardsList[1].Suit == PlayersList[ForWhichPlayer - 1].CardsList[2].Suit)
        {
            PlayersList[ForWhichPlayer - 1].NoOfSameSuit = 3;
        }
        else
        if(PlayersList[ForWhichPlayer - 1].CardsList[0].Suit == PlayersList[ForWhichPlayer - 1].CardsList[1].Suit ||
            PlayersList[ForWhichPlayer - 1].CardsList[1].Suit == PlayersList[ForWhichPlayer - 1].CardsList[2].Suit ||
            PlayersList[ForWhichPlayer - 1].CardsList[2].Suit == PlayersList[ForWhichPlayer - 1].CardsList[0].Suit)
            {
                PlayersList[ForWhichPlayer - 1].NoOfSameSuit = 2;
            }
            else
            {
                PlayersList[ForWhichPlayer - 1].NoOfSameSuit = 0;
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

    public int AssignDeckLayer(int ForWhichPlayer)  //Gives integer of Layer it belongs. From 1 to 6.
    {


        if (PlayersList[ForWhichPlayer - 1].NoOfSameRank==3) //Layer 1 //Three Cards Of Same Rank.
        {
            return 1;
        }
        else if(PlayersList[ForWhichPlayer - 1].Ordered==true && PlayersList[ForWhichPlayer - 1].NoOfSameSuit == 3) //Layer 2 // Consecutive Cards && Same Suit.
        {
            return 2;
        }
        else if (PlayersList[ForWhichPlayer - 1].Ordered == true && PlayersList[ForWhichPlayer - 1].NoOfSameSuit != 3) //Layer 3 // Consecutive Cards && Diffrent Suit.
        {
            return 3;
        }
        else if (PlayersList[ForWhichPlayer - 1].Ordered == false && PlayersList[ForWhichPlayer - 1].NoOfSameSuit == 3) //Layer 4 // Non-Consecutive Cards && Same Suit.
        {
            return 4;
        }
        else if (PlayersList[ForWhichPlayer - 1].NoOfSameRank == 2) //Layer 5 // Two Cards of Same Rank.
        {
            return 5;
        }
        else //Layer 6 //
        {
            return 6;
        }

        //return 0; // Never going to happen.
    }


    public int AssignDeckSubLayer(int ForWhichPlayer, int LayerNumber)  //
    {
        switch(LayerNumber)
        {
            case 1:
                if(PlayersList[ForWhichPlayer-1].CardsList[0].Rank == 1)
                {
                    return 1;  //AAA as 1.
                }
                else
                {
                    return (15 - PlayersList[ForWhichPlayer - 1].CardsList[0].Rank); //KKK(13) as 2, QQQ(12) as 3 , .... 222(2) as 13.
                }
            case 2:
                if(PlayersList[ForWhichPlayer - 1].CardsList[0].Rank == 13 &&
                            PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == 12 &&
                                    PlayersList[ForWhichPlayer - 1].CardsList[2].Rank == 1)
                {
                    return 1;  // KQA
                }
                else if(PlayersList[ForWhichPlayer - 1].CardsList[0].Rank == 3 &&
                                PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == 2 &&
                                    PlayersList[ForWhichPlayer - 1].CardsList[2].Rank == 1)
                {
                    return 2;  //32A
                }
                else
                {
                    return (16 - PlayersList[ForWhichPlayer - 1].CardsList[0].Rank); //13,12,11 -  12,11,10 - 10,9,8
                }
            case 3:
                if (PlayersList[ForWhichPlayer - 1].CardsList[0].Rank == 13 &&
                            PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == 12 &&
                                    PlayersList[ForWhichPlayer - 1].CardsList[2].Rank == 1)
                {
                    return 1;  // KQA
                }
                else if (PlayersList[ForWhichPlayer - 1].CardsList[0].Rank == 3 &&
                                PlayersList[ForWhichPlayer - 1].CardsList[1].Rank == 2 &&
                                    PlayersList[ForWhichPlayer - 1].CardsList[2].Rank == 1)
                {
                    return 2;  //32A
                }
                else
                {
                    return (16 - PlayersList[ForWhichPlayer - 1].CardsList[0].Rank); //13,12,11 -  12,11,10 - 10,9,8
                }
            default:  //FOR 4,5,6 //THIS WILL BE CHECKED WHILE RANKING.
                    return 0;
        }
    }

    public void RankPlayers(int ForWhichPlayer)  //RANKING ALGORITHM.
    {
        int CurrentPosition = 0;
        TopRankers.Insert(0, PlayersList[ForWhichPlayer - 1]);

        void Swap()
        {
            CardList temp = TopRankers[CurrentPosition+1];
            TopRankers.RemoveAt(CurrentPosition + 1);
            TopRankers.Insert(CurrentPosition, temp);
            CurrentPosition += 1;
        }

        for(int i= CurrentPosition; i<(TopRankers.Count-1); i++)
        {
            if(TopRankers[CurrentPosition].DeckLayer > TopRankers[CurrentPosition + 1].DeckLayer)   //SORT WITH HELP OF LAYERS INT.
            {
                Swap();
            }
            else
            {
                break;
            }
        }

        for (int i = CurrentPosition; i < (TopRankers.Count - 1); i++)
        {
            if (TopRankers[CurrentPosition].DeckLayer == TopRankers[CurrentPosition + 1].DeckLayer)
            {
                if (TopRankers[CurrentPosition].DeckLayer <= 3)
                {
                    //THEY ARE 1, 2, 3
                    if (TopRankers[CurrentPosition].DeckLayer > TopRankers[CurrentPosition + 1].DeckLayer)  //If Smaller , Insert Above
                    {
                        CardList temp = TopRankers[CurrentPosition];
                        TopRankers.Insert(CurrentPosition, TopRankers[CurrentPosition + 1]);
                        TopRankers.Insert(CurrentPosition + 1, temp);
                        CurrentPosition += 1;
                    }
                }
                else //THEY ARE 4,5,6
                {
                    if (TopRankers[CurrentPosition].CardsList[0].Rank > TopRankers[CurrentPosition + 1].CardsList[0].Rank) //IF GREATER
                    {
                        //swap
                        Swap();
                    }
                    else if (TopRankers[CurrentPosition].CardsList[0].Rank == TopRankers[CurrentPosition + 1].CardsList[0].Rank) //IF EQUAL
                    {
                        //check 2nd card
                        if (TopRankers[CurrentPosition].CardsList[1].Rank > TopRankers[CurrentPosition + 1].CardsList[1].Rank) //IF GREATER
                        {
                            //swap
                            Swap();
                        }
                        else if (TopRankers[CurrentPosition].CardsList[1].Rank == TopRankers[CurrentPosition + 1].CardsList[1].Rank) //IF EQUAL
                        {
                            //check 3nd card
                            if (TopRankers[CurrentPosition].CardsList[2].Rank > TopRankers[CurrentPosition + 1].CardsList[2].Rank) //IF GREATER
                            {
                                //swap
                                Swap();
                            }
                        }
                        else {/*DO NOTHING*/  break; }
                    }
                    else {/*DO NOTHING*/  break; }
                }

            }
            else
            {
                break;
            }
        }
    }

}
