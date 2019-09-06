using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    public int botManager(int playernumber, int priority)
    {
        if (playernumber <= 2 && priority == 1)
        {
            return 1;
        }
        else
        {
            if(playernumber>2 && priority == 1)
            {
                return 1;
            }
            else
            {
                return Random.Range(3, 6);
            }
        }
    }
}