using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour


    
{


    public static gameManager Instance { get; set; }
    // Start is called before the first frame update
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
       

        
    }


}
