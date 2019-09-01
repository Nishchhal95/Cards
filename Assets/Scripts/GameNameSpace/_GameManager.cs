using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
namespace GameNameSpace
{
    public class _GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public Transform[] InstantiatePosition;
        public static int numberOfPlayer = 5;

        //-----
        public Button[] ControlButtons;
        //------

        public List<GameObject> PlayersList;
        private int PlayerIndex;

        public Text TurnIndicator;
        public Text Debugger;
        //-----
        private bool PrimaryPlayerDead = false;

        public Slider BettingSlider;
        public Text BettingValue;

        public Sprite DummySprite;

        private void Start()
        {
            PlayerIndex = Random.Range(1, numberOfPlayer + 1);
            CardsManager.instance.MakeDatabase();
            CreatePlayers(numberOfPlayer);

            TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            ChangeSelectionUI();
            ChangeButtonState();
            Utils.DoActionAfterSecondsAsync(StartGame, 3f);

            BettingSlider.minValue = 40;
            BettingSlider.maxValue = 100;
        }


        public void RestartGame()
        {
            SceneManager.LoadScene("Player");
        }

        private void CreatePlayers(int playerNumbers)
        {

            for (int i = 1; i <= playerNumbers; i++)
            {
                if (i == 1)
                {
                    CreatePlayer(FB_Handler.instance.SavedUsername + i, 100, 1000, FB_Handler.instance.SavedProfile, i);
                }
                else
                {
                    CreatePlayer("Player " + i, 100, 1000, DummySprite, i);
                }

            }
        }

        private void CreatePlayer(string playerName, int chips, float XP, Sprite sprite, int playernumber)
        {
            GameObject player = Instantiate(playerPrefab, InstantiatePosition[playernumber - 1]);
            PlayersList.Add(player);
            Player playerScript = player.GetComponent<Player>();

            if (playerScript != null)
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
            Debugger.text = "Player " + PlayerIndex + " clicked : SHOW";

            if (PlayerIndex < PlayersList.Count)
            {
                PlayerIndex++;
            }
            else
            {
                PlayerIndex = 1;
            }

            TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            StartGame();
        }

        public void Bet(bool IsPlayer)
        {
            if(IsPlayer==true)
            {
                Debugger.text = "Player " + PlayerIndex + " Betted " + BettingSlider.value + " Chips";
            }
            else
            {
                Debugger.text = "Player " + PlayerIndex + " clicked : BET";
            }
          
            if (PlayerIndex < PlayersList.Count)
            {
                PlayerIndex++;
            }
            else
            {
                PlayerIndex = 1;
            }
            TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            StartGame();
        }

        public void Fold(bool IsPlayer)
        {
            if(IsPlayer==true)
            {
                PrimaryPlayerDead = true;
            }

            Debugger.text = "Player " + PlayerIndex + " clicked : FOLD";

            Destroy(PlayersList[PlayerIndex - 1].gameObject);
            PlayersList.RemoveAt(PlayerIndex - 1);

            for (int i = (PlayerIndex - 1); i < PlayersList.Count; i++)
            {
                PlayersList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Player " + (i + 1);
            }

            if (PlayerIndex == PlayersList.Count + 1)
            {
                PlayerIndex = 1;
            }

            TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            StartGame();
        }

        private void StartGame()
        {
            ChangeButtonState();
            ChangeSelectionUI();

            if (PrimaryPlayerDead == true)  //We are dead, Go On.
            {
              
                Utils.DoActionAfterSecondsAsync(StartPlayersTurn, 3f);
            }
            else  //We are not dead.
            {
                if (PlayerIndex != 1)  //If index is not 1 , Go On.
                {
                  
                    Utils.DoActionAfterSecondsAsync(StartPlayersTurn, 3f);
                }
            }

        }

      

        private void StartPlayersTurn()
        {
            int R = Random.Range(1, 4);  //Random function is exclusive function. So, Will return value from 1 to 3 only.
            switch (R)
            {
                case 1:
                    Show();
                    break;
                case 2:
                    Bet(false);
                    break;
                case 3:
                    Fold(false);
                    break;
            }
        }

        private void ChangeSelectionUI()
        {
            for(int i=0; i<PlayersList.Count; i++)
            {
                if((i+1)== PlayerIndex)
                {
                    PlayersList[i].GetComponent<Player>().SelectionUI.gameObject.SetActive(true);
                }
                else
                {
                    PlayersList[i].GetComponent<Player>().SelectionUI.gameObject.SetActive(false);
                }
               
            }
        }

        private void ChangeButtonState()
        {
            bool Interactable;

            if (PrimaryPlayerDead == true)  //We are dead, Go On.
            {
                Interactable = false;
            }
            else  //We are not dead.
            {
                if (PlayerIndex != 1)  //If index is not 1 , Go On.
                {
                    Interactable = false;
                }
                else  //If index is 1, Wait Player to play. 
                {
                    Interactable = true;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                ControlButtons[i].interactable = Interactable;
            }
        }

        public void BetUpdate()
        {
            BettingValue.text = BettingSlider.value.ToString();
        }
    }
}
