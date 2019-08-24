using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Blind();
            Indexchange();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Show();
            Indexchange();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Fold();
            Indexchange();
        }
    }

    void Blind()
    {
        print("Player " + PlayerCreater.indexInPlay + " Plays Blind");
    }

    void Show()
    {
        Debug.Log("Player " + PlayerCreater.indexInPlay + " Plays Show");
    }

    void Fold()
    {
        Debug.Log("Player " + PlayerCreater.indexInPlay + " Plays Fold s");
    }

    void Indexchange()
    {
        if (PlayerCreater.indexInPlay ==PlayerCreater.playercount)
        {
            PlayerCreater.indexInPlay = 0;
        }
        else
        {
            PlayerCreater.indexInPlay++;
        }
    }  
}
