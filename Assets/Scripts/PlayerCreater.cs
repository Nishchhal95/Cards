using System.Collections.Generic;
using UnityEngine;

public class PlayerCreater : MonoBehaviour
{
    public List<Transform> placeholder;

    int playercount;

    public GameObject player;

    private void Awake()
    {
        playercount = 0;
    }


    private void Start()
    {
        Placeplayer();
    }


    private void Placeplayer()
    {
        playercount = Random.Range(1, 6);
        print(playercount);
        for(int i =0; i<playercount;i++)
        {
            Instantiate(player,placeholder[i].transform.position,player.transform.rotation);
        }
    }
}
