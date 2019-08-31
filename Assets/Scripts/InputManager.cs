using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameNameSpace;

public class InputManager : MonoBehaviour
{
    public PlayerCreater playercreater;
    public  TextMeshProUGUI textturn;
    Player player;

    private void Start()
    {
      // playercreater = GetComponent<PlayerCreater>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.blind)
            BetButton();
        else if (player.show)
            ShowButton();
        else if (player.fold)
            FoldButton();
    }

    public void BetButton() // Blind Ui Button
    {
        playercreater.playerInfo[PlayerCreater.indexInPlay].Blind();
        textturn.text = "Player " + PlayerCreater.indexInPlay.ToString() + " Plays BLIND";
        Indexchange();
    }

    public void FoldButton() // Fold UI Button
    {
        playercreater.playerInfo[PlayerCreater.indexInPlay].Fold();
        textturn.text = "Player " + PlayerCreater.indexInPlay.ToString() + " Plays FOLD";
        Indexchange();
    }

    public void ShowButton() // Show UI Button
    {
        playercreater.playerInfo[PlayerCreater.indexInPlay].Show();
        textturn.text = "Player " + PlayerCreater.indexInPlay.ToString() + " Plays SHOW";
        Indexchange();
    }

    public void Indexchange()
    {
        print("playercount "+ _GameManager.numberOfPlayer);
        if (PlayerCreater.indexInPlay == _GameManager.numberOfPlayer - 1)
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
