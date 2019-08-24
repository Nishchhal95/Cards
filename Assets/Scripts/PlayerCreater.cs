using System.Collections.Generic;
using UnityEngine;

public class PlayerCreater : MonoBehaviour
{
    public List<Transform> placeholder;
    public List<Transform> place;
    public List<GameObject> players;

    int playercount;
    int indexInPlay;                                                                                               
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
        if(Input.GetKeyDown(KeyCode.B))
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
        print("Player " + indexInPlay + " Plays Blind");
    }

    void Show()
    {
        Debug.Log("Player " + indexInPlay + " Plays Show");
    }

    void Fold()
    {
        Debug.Log("Player " + indexInPlay + " Plays Fold");
    }

   void Indexchange()
    {
       if(indexInPlay==playercount)
        {
            indexInPlay=0;
        }
       else
        {
            indexInPlay++;
        }
    }

}
 