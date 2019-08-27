using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
     public Player player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
           player.Blind();
            player.Indexchange();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            player.Show();
            player.Indexchange();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Fold();
            player.Indexchange();
        }
    }
}
