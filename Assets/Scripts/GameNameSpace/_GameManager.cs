using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameNameSpace
{
    public class _GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public Transform[] InstantiatePosition;
        public static int numberOfPlayer = 5;

        //-----

        public List<GameObject> PlayersList;
        private int PlayerIndex = 1;

        public Text TurnIndicator;
        public Text Debugger;
        //-----
        private int CurrentPlayer;

        private void Start()
        {
            CurrentPlayer = Random.Range(1, numberOfPlayer);
            CardsManager.instance.MakeDatabase();
            CreatePlayers(numberOfPlayer);
            TurnIndicator.text = "It's turn of Player " + PlayerIndex + " to play";
        }

        private void CreatePlayers(int playerNumbers)
        {
            
            for (int i = 1; i <= playerNumbers; i++)
            {
                if(i==CurrentPlayer)
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
            GameObject player = Instantiate(playerPrefab, InstantiatePosition[playernumber-1]);
            PlayersList.Add(player);
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

        public void Show()
        {
            Debugger.text = "Player " + PlayerIndex + "clicked : SHOW";

            if(PlayerIndex < PlayersList.Count)
            {
                PlayerIndex++;
            }
            else
            {
                PlayerIndex = 1;
            }
                
           
            TurnIndicator.text = "It's turn of Player " + PlayerIndex + " to play";
        }

        public void Bet()
        {
            Debugger.text = "Player " + PlayerIndex + "clicked : BET";
            if (PlayerIndex < PlayersList.Count)
            {
                PlayerIndex++;
            }
            else
            {
                PlayerIndex = 1;
            }
            TurnIndicator.text = "It's turn of Player " + PlayerIndex + " to play";
        }
        public void Fold()
        {
            Debugger.text = "Player " + PlayerIndex + "clicked : FOLD";

            Destroy(PlayersList[PlayerIndex - 1].gameObject); 
            PlayersList.RemoveAt(PlayerIndex - 1);

            for(int i = (PlayerIndex-1); i < PlayersList.Count; i++)
            {
                    PlayersList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Player " + (i + 1);
            }

            if (PlayerIndex == PlayersList.Count)
            {
                PlayerIndex = 1;
            }
            TurnIndicator.text = "It's turn of Player " + PlayerIndex + " to play";
        }
        
    }
}
