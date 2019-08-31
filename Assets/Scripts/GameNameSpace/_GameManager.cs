using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameNameSpace
{
    public class _GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public Transform[] InstantiatePosition;
        public static int numberOfPlayer = 5;

        private void Start()
        {
            CardsManager.instance.MakeDatabase();
            CreatePlayers(numberOfPlayer);
        }

        private void CreatePlayers(int playerNumbers)
        {
            for (int i = 0; i < playerNumbers; i++)
            {
                if(i==0)
                {
                    CreatePlayer(FB_Handler.instance.FB_UserName.text.ToString() + i, 100, 1000, FB_Handler.instance.FB_Profile.sprite, i);
                }
                else
                {
                    CreatePlayer("Player " + i, 100, 1000, null, i);
                }
               
            }
        }

        private void CreatePlayer(string playerName, int chips, float XP, Sprite sprite, int playernumber)
        {
            GameObject player = Instantiate(playerPrefab, InstantiatePosition[playernumber]);

            Player playerScript = player.GetComponent<Player>();

            if(playerScript != null)
            {
                //We Assign Data to it
                playerScript.playerName = playerName;
                playerScript.chips = chips;
                playerScript.XP = XP;
                playerScript.playerSprite = sprite;

                playerScript.cardList = CardsManager.instance.Get3Cards();
                playerScript.PopulateData();
                playerScript.PopulateCards();
            }
        }
    }
}
