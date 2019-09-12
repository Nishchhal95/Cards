using UnityEngine;

public class BotManager : MonoBehaviour
{
    public int botManager(int playernumber, int priority)
    {
        if (playernumber <= 2 && priority == 1)
        {
            return 3;
        }
        else
        {
            if(playernumber>2 && priority == 2)
            {
                return 2;
            }
            else
            {
                if(playernumber == 2)
                {
                    return 1;
                }
                else
                {
                    return 1;
                }

            }
        }
    }
}