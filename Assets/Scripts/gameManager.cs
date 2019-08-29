using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    // game manager for turn logic
    public static gameManager Instance { get; set; }
    public static int playercount; // Count of active player

    void Awake()
    {
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
        playercount = Random.Range(2, 5);
    }

   
}
