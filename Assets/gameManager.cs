using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour

// game manager for turn logic


    
{
    Player player;
    bool playerActive = false;

    public int playerWhoseTurnActive;
    public static gameManager Instance { get; set; }


    void Awake()


    {

        player = GetComponent<Player>();


        if (Instance == null)

        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        else
        {
            Destroy(gameObject);
        }
    }
   

    void Start()
    {
       

        
    }


    void PlayerTurnFunction()

        //this method can be set active only for the player whose turn is on
    {
        if(playerActive)

        {
            player.Blind();
            player.Fold();
            player.Indexchange();
        }
        
    }

}
