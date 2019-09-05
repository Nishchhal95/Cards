using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static GameInstance new_instance = null;

    public int MinimumBettingValue;

    private void Awake()
    {
        if (new_instance == null)
        {
            new_instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

}
