using System.Collections.Generic;
using UnityEngine;

public class PlayerCreater : MonoBehaviour
{
    public List<RectTransform> placeholder; //PlaceHolder for player at Canvas
    public List<RectTransform> place; //Temp List for holding pos
    public List<GameObject> players;// List Of Gameobject of player UI
   
    public List<Player> playerInfo; //List of Player Class Which Having Info

   
    public static int indexInPlay; //  index of plyaer from list whose turn is in active                                                                                           
    

    private void Awake()
    {
        indexInPlay = 0;
      
    }


    private void Start()
    {
        place = placeholder;
        Placeplayer();
        CreatePlayer();
        print(playerInfo.Count);
        AssingCards();
    }


    private void Placeplayer() // Randomly place the plyaer at the table or Placeholder
    {
      
        int index = 4;
        for(int i =0; i<gameManager.playercount;i++)
        {
            int posindex = Random.Range(0, index--);
            players[index].SetActive(true);
            players[index].transform.position = placeholder[posindex].position;
            place.RemoveAt(posindex);
        }
    }

    public void CreatePlayer() // Creating the object of the Player Class with info and adding to list  
    {
       for(int i =0; i<=gameManager.playercount; i++)
        {
            Player player = new Player();
            playerInfo.Add(player); 
        }
    }

    public void AssingCards()
    {
        print(cardsLogic.Instance.PlayersList.Count);
        for(int i =0;i< gameManager.playercount; i++)
        {
            playerInfo[i].cards = cardsLogic.Instance.PlayersList[i].CardsList;
        }
    }

}
 