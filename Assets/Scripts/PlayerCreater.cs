using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNameSpace;

public class PlayerCreater : MonoBehaviour
{

    public List<RectTransform> placeholder; //PlaceHolder for player at Canvas
   // public List<RectTransform> place; //Temp List for holding pos
    public List<GameObject> players;// List Of Gameobject of player UI
   
    public  List<Player> playerInfo; //List of Player Class Which Having Info

    public List<Image> Playersprites;
   
    public static int indexInPlay; //  index of plyaer from list whose turn is in active                                                                                           

    public CardsManager CardsManager;

    private void Awake()
    {
        indexInPlay = 0;
        CardsManager = GetComponent<CardsManager>();
    }


    private void Start()
    {
    //    place = placeholder;
        Placeplayer();
        CreatePlayer();
        print(playerInfo.Count);
        AssingCards();
    }


    private void Placeplayer() // Randomly place the plyaer at the table or Placeholder
    {
      
       // int index = 4;
        for(int i =0; i<_GameManager.numberOfPlayer;i++)
        {
           // int posindex = Random.Range(0, index--);
            players[i].SetActive(true);
            players[i].transform.position = placeholder[i].position;
          //  place.RemoveAt(i);
        }
    }

    public void CreatePlayer() // Creating the object of the Player Class with info and adding to list  
    {
       for(int i =0; i< _GameManager.numberOfPlayer; i++)
        {
            Player player = new Player();
            playerInfo.Add(player); 
        }
    }

    public void AssingCards()  // Assigning Cards to specific Players
    {
        //print(CardsManager.PlayersList.Count);
        for(int i =0; i< _GameManager.numberOfPlayer; i++)
        {
            playerInfo[i].cards = CardsManager.PlayersList[i].CardsList;
        }
        cardSpritestoplayer();
    }

    public void cardSpritestoplayer()
    {
        for(int player = 0, sprites =0 ; player< _GameManager.numberOfPlayer; player++)
        {
            for(int card =0; card<3; card++)
            {
                print(sprites.ToString() + player.ToString() + card.ToString());
                print("Countlist" + CardsManager.PlayersList.Count);
                Playersprites[sprites].sprite = CardsManager.PlayersList[player].CardsList[card].CardSprite;
                sprites++;
            }
        }
    }

}
 