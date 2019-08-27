using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour

// game manager for turn logic


    
{
    Player player;

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
    {
        player.Blind();
        player.Fold();
        player.Indexchange();
    }

}
