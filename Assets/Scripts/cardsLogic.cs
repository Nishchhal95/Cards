using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class cardsLogic : MonoBehaviour




{



    public List<string> cardDeck;// dynamic array to hold/ store  the cards 

    // later it can be changed to list of objects


    
   public int playerNo;     // no of people to assign 3 cards to them.

    int k;  // that will randomly fetch cards from database



    void Start()


    {
        AssignCardToXpeople(); 


    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    public void assignCArds()
    {
        // printing  3 random cards for a player

        for (int i=0;i<3;i++)


        {
         

            k = Random.Range(0, cardDeck.Count);  // get random index from list

            Debug.Log(cardDeck[k]);    //fetching value from cards database


            cardDeck.RemoveAt(k);  // avoid repeattion of aassignment of cards




        }
      
      
    }



    public void AssignCardToXpeople()


    {
        Debug.Log("Assigning three random cards to" + playerNo + " number of players : " );



        for(int i=1;i<=playerNo;i++)

        {
            Debug.Log("assigning 3 cards to player number  " + i ); // so that its known which card assigned to which player

            assignCArds();
        }




        
    }
}
