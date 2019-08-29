using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreater : MonoBehaviour
{
    public List<RectTransform> placeholder; //PlaceHolder for player at Canvas
   // public List<RectTransform> place; //Temp List for holding pos
    public List<GameObject> players;// List Of Gameobject of player UI
   
    public List<Player> playerInfo; //List of Player Class Which Having Info

    public List<Image> Playersprites;
   
    public static int indexInPlay; //  index of plyaer from list whose turn is in active                                                                                           
    

    private void Awake()
    {
        indexInPlay = 0;
      
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
        for(int i =0; i<gameManager.playercount;i++)
        {
           // int posindex = Random.Range(0, index--);
            players[i].SetActive(true);
            players[i].transform.position = placeholder[i].position;
          //  place.RemoveAt(i);
        }
    }

    public void CreatePlayer() // Creating the object of the Player Class with info and adding to list  
    {
       for(int i =0; i<gameManager.playercount; i++)
        {
            Player player = new Player();
            playerInfo.Add(player); 
        }
    }

    public void AssingCards()  // Assigning Cards to specific Players
    {
        print(cardsLogic.Instance.PlayersList.Count);
        for(int i =0; i<gameManager.playercount; i++)
        {
            playerInfo[i].cards = cardsLogic.Instance.PlayersList[i].CardsList;
        }
        cardSpritestoplayer();
    }

    public void cardSpritestoplayer()
    {
        for(int player = 0, sprites =0 ; player<gameManager.playercount;player++)
        {
            for(int card =0; card<3; card++)
            {
                print(sprites.ToString() + player.ToString() + card.ToString());
                print("Countlist" + cardsLogic.Instance.PlayersList.Count);
                Playersprites[sprites].sprite = cardsLogic.Instance.PlayersList[player].CardsList[card].CardSprite;
                sprites++;
            }
        }
    }

}
 