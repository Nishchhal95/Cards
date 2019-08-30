using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // game manager for turn logic
    public static GameManager Instance { get; set; }
    public static int PlayerCount; // Count of active player

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
        PlayerCount = Random.Range(2, 5);
    }

   
}
