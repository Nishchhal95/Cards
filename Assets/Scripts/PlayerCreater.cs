using System.Collections.Generic;
using UnityEngine;

public class PlayerCreater : MonoBehaviour
{
    public List<Transform> placeholder;
    public List<Transform> place;
    public List<GameObject> players;

   public static int playercount;
    public static int indexInPlay;                                                                                               
    public GameObject player;

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
        playercount = Random.Range(1,6);
        print(playercount);
        int index = 5;
        for(int i =0; i<playercount;i++)
        {
            int posindex = Random.Range(0, index--);
            GameObject go = Instantiate(player,place[posindex].transform.position,player.transform.rotation);
            players.Add(go);
            place.RemoveAt(posindex);
        }
    }

    private void Update()
    {
      
    }

  

}
 