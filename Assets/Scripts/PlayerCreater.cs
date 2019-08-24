using System.Collections.Generic;
using UnityEngine;

public class PlayerCreater : MonoBehaviour
{
    public List<Transform> placeholder;
    public List<Transform> place;

    int playercount;

    public GameObject player;

    private void Awake()
    {
        playercount = 0;
    }


    private void Start()
    {
        place = placeholder;
        Placeplayer();
    }


    private void Placeplayer()
    {
        playercount = Random.Range(1,6);
        print(playercount);
        int index = 5;
        for(int i =0; i<playercount;i++)
        {
            int posindex = Random.Range(0, index--);
            Instantiate(player,place[posindex].transform.position,player.transform.rotation);
            place.RemoveAt(posindex);
        }
        place = placeholder;
    }
}
