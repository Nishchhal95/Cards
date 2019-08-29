using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputManager : MonoBehaviour
{
     public PlayerCreater playercreater;
    public  TextMeshProUGUI textturn;

    private void Awake()
    {
        playercreater = GetComponent<PlayerCreater>();    
    }

    public void BetButton() // Bet Ui Button
    {
        playercreater.playerInfo[PlayerCreater.indexInPlay].Blind();
        textturn.text = "Player " + PlayerCreater.indexInPlay.ToString() + " Plays Blind";
        Indexchange();
    }

    public void FoldButton() // Fold UI Button
    {
        playercreater.playerInfo[PlayerCreater.indexInPlay].Fold();
        textturn.text = "Player " + PlayerCreater.indexInPlay.ToString() + " Plays Blind";
        Indexchange();
    }

    public void ShowButton() // Show UI Button
    {
        playercreater.playerInfo[PlayerCreater.indexInPlay].Show();
        textturn.text = "Player " + PlayerCreater.indexInPlay.ToString() + " Plays Blind";
        Indexchange();
    }

    public void Indexchange()
    {
        print("playercount "+gameManager.playercount);
        if (PlayerCreater.indexInPlay == gameManager.playercount)
        {
            PlayerCreater.indexInPlay = 0;
            print(PlayerCreater.indexInPlay);
        }
        else
        {
            PlayerCreater.indexInPlay++;
            print("IndexPlay " + PlayerCreater.indexInPlay);
        }
    }
}
