using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameNameSpace
{
    public class _GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public int numberOfPlayer = 3;

        private void Start()
        {
            CreatePlayers(numberOfPlayer);
        }

        private void CreatePlayers(int playerNumbers)
        {
            for (int i = 0; i < playerNumbers; i++)
            {
                CreatePlayer("Player " + i, 100, 1000, null);
            }
        }

        private void CreatePlayer(string playerName, int chips, float XP, Sprite sprite)
        {
            GameObject player = Instantiate(playerPrefab, transform);

            Player playerScript = player.GetComponent<Player>();

            if(playerScript != null)
            {
                //We Assign Data to it
                playerScript.playerName = playerName;
                playerScript.chips = chips;
                playerScript.XP = XP;
                playerScript.playerSprite = sprite;

                playerScript.PopulateData();
            }
        }
    }
}
