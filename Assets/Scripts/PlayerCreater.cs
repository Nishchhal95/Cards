using System.Collections.Generic;
using UnityEngine;

public class PlayerCreater : MonoBehaviour
{
    public List<RectTransform> placeholder;
    public List<RectTransform> place;
    public List<GameObject> players;

   public static int playercount;
    public static int indexInPlay;                                                                                               

    private void Awake()
    {
        indexInPlay = 0;
        playercount = 0;
    }


    private void Start()
    {
        place = placeholder;
        Placeplayer();
    }


    private void Placeplayer()
    {
        playercount = Random.Range(1,5);
        print(playercount);
        int index = 4;
        for(int i =0; i<playercount;i++)
        {
            int posindex = Random.Range(0, index--);
            players[index].SetActive(true);
            players[index].transform.position = placeholder[posindex].position;
            place.RemoveAt(posindex);
        }
    }

}
 