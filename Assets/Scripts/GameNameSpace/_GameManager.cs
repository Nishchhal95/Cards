using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace GameNameSpace
{
    public class _GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public Transform[] InstantiatePosition;
        public static int numberOfPlayer = 5;

        public List<GameObject> ObjectsToDisable;
        
        public List<GameObject> PlayersList;
       
        public Text TurnIndicator;
        public Text Debugger;

        public TMP_Text TotalPotText;
        public int TotalPot = 0;

        public Sprite DummySprite;  //Sprite for Other Palyers.

        //-----For Main Player---------
        public Image MainPlayerImage;  
        public TMP_Text MainPlayerName;
        public TMP_Text MainPlayerChips;
        public TMP_Text BettingValueText;
        public Slider BettingSlider;

        //---Private Variables-----------

        private bool PrimaryPlayerDead = false;
        private int PlayerIndex;
        private GameObject MainPlayer;

        //-----
        public int MinimumBettingValue = 40;
        private void Start()
        {
            PlayerIndex = Random.Range(1, numberOfPlayer + 1);
            CardsManager.instance.MakeDatabase();
            CreatePlayers(numberOfPlayer);

            TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            ChangeSelectionUI();
            ChangeButtonState();
            Utils.DoActionAfterSecondsAsync(StartGame, 3f);

            //Will be in set slider.
           
            RefreshSlider();
            RefreshPotText();

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
                    if (FB_Handler.instance.SavedProfile!=null)
                    {
                        CreatePlayer(FB_Handler.instance.SavedUsername, 100, 1000, FB_Handler.instance.SavedProfile, i);
                    }
                   
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

            playerScript.name = playerName;
            playerScript.coin = chips;
            playerScript.XP = XP;
            playerScript.playerSprite = sprite;

            playerScript.cardList = CardsManager.instance.Get3Cards();


            if (playernumber==1)   //For Main Player
            {
                MainPlayer = player;  //Set Main Player.
                if (playerScript != null)
                {
                    //We Assign Data to it
                    MainPlayerName.text = playerScript.name;
                    MainPlayerChips.text = playerScript.coin.ToString();
                    MainPlayerImage.sprite = playerScript.playerSprite;
                    player.transform.GetChild(0).transform.Find("Name").gameObject.SetActive(false);
                    player.transform.GetChild(0).transform.Find("Chips").gameObject.SetActive(false);
                    player.transform.GetChild(0).transform.Find("XP").gameObject.SetActive(false);
                    player.transform.GetChild(0).transform.Find("MaskProfile").gameObject.SetActive(false);
                    playerScript.PopulateCards();
                }
            }
            else   //For Other Players
            {
                if (playerScript != null)
                {
                    playerScript.PopulateData();
                    playerScript.PopulateCards();
                }
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

                MainPlayer.GetComponent<Player>().coin -= (int)BettingSlider.value;
                TotalPot += (int)BettingSlider.value;
                //Refresh Data after some Maths..
                MainPlayer.GetComponent<Player>().RefreshData();
                RefreshSlider();
                RefreshChipsText();
                RefreshPotText();
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

            if(PlayersList.Count!=1) //Game move forward, only if there are more than one player.
            {
                if (PrimaryPlayerDead == true)  //We are dead, Go On in Loop.
                {
              
                    Utils.DoActionAfterSecondsAsync(StartPlayersTurn, 3f);
                }
                else  //We are not dead.
                {
                    if (PlayerIndex != 1)  //If index is not 1 , Go On.
                    {
                  
                        Utils.DoActionAfterSecondsAsync(StartPlayersTurn, 3f);
                    } 
                    //else wait for player 1 to play
                }
            }
            else  // Some Player Won...
            {
                Debugger.text = "Yeah !! Player " + PlayerIndex + " won !";
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
                    if(PrimaryPlayerDead==true)
                    {
                        PlayersList[i].GetComponent<Player>().SelectionUI.gameObject.SetActive(true);
                    }
                    else
                    {
                        if(PlayerIndex!=1)
                        {
                            PlayersList[i].GetComponent<Player>().SelectionUI.gameObject.SetActive(true);
                        }
                    }
                    
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

            for (int i = 0; i < ObjectsToDisable.Count ; i++)
            {
                if(ObjectsToDisable[i].GetComponent<Button>()==true)
                {
                    ObjectsToDisable[i].GetComponent<Button>().interactable = Interactable;
                }
                else
                {
                    ObjectsToDisable[i].GetComponent<Slider>().interactable = Interactable;
                }
            }
        }

        
        public void BetUpdate()
        {
            BettingValueText.text = BettingSlider.value.ToString();
        }

        public void AddBetButton()
        {
            BettingSlider.value += 2;
            BettingValueText.text = BettingSlider.value.ToString();
        }
        public void SubstractBetButton()
        {
            BettingSlider.value -= 2;
            BettingValueText.text = BettingSlider.value.ToString();
        }

        public void RefreshSlider()
        {
            BettingSlider.minValue = MinimumBettingValue;
            BettingSlider.maxValue = MainPlayer.GetComponent<Player>().coin;
        }

        public void RefreshPotText()
        {
            TotalPotText.text = "Total Pot : " + TotalPot.ToString();
        }

        public void RefreshChipsText()
        {
            MainPlayerChips.text = MainPlayer.GetComponent<Player>().coin.ToString();
        }

        public void ChangeScene(string Scene)
        {
            SceneManager.LoadScene(Scene);
        }
    }
}
