using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json;

namespace GameNameSpace
{
    public class _GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public Transform[] InstantiatePosition;
        public static int numberOfPlayer;

        public List<GameObject> ObjectsToDisable;
        
        public List<GameObject> PlayersList;
       
        public TMP_Text TurnIndicator;
        public TMP_Text Debugger;

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
        public float WaitingTime = 5f;   //Waiting Time for all the actions.

        private int MainPlayerTimeoutIndex = 0;  // Index to stop Mismatching 45 seconds of Previous Turn and Current Turn.

        private void Start()
        {
            WebRequestManager.HttpGetPlayerData((List<GameNameSpace.Player> NewPlayerList) =>
            {
                numberOfPlayer = NewPlayerList.Count + 1;
                SecondStart();
            });

        }

        private void SecondStart()
        {
            PlayerIndex = Random.Range(1, numberOfPlayer + 1);
            CardsManager.instance.MakeDatabase();
            CreatePlayers(numberOfPlayer);
        }

        private void ThirdStart()
        {

            //TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            TurnIndicator.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " is playing...";
            ChangeSelectionUI();
            ChangeButtonState();
            RefreshSlider();
            RefreshPotText();

            Utils.DoActionAfterSecondsAsync(StartGame, WaitingTime);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Player");
        }

        private void CreatePlayers(int playerNumbers)
        {
            WebRequestManager.HttpGetPlayerData((List<GameNameSpace.Player> NewPlayerList) =>
            {
                //Call Your Create Player Method From Here and Pass playerList there and loop through that playerList

                for (int i = 1; i <= playerNumbers; i++)
                {
                    if (i == 1)
                    {
                        if (FB_Handler.instance.SavedProfile != null)
                        {
                            CreatePlayer(FB_Handler.instance.SavedUsername,100, 1000, FB_Handler.instance.SavedProfile, i);
                        }

                    }
                    else
                    {
                        //Here i starts from 2.
                        CreatePlayer( NewPlayerList[i-2].name , NewPlayerList[i-2].coin , 100, DummySprite, i);
                    }

                }

                ThirdStart();
            });

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


                    //Main Player will get its own UI.
                    playerScript.nameText.gameObject.SetActive(false);
                    playerScript.chipsText.gameObject.transform.parent.gameObject.SetActive(false);
                    playerScript.xpText.gameObject.SetActive(false);
                    playerScript.profileImage.gameObject.transform.parent.gameObject.SetActive(false);

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
            //Debugger.text =  "Player " + PlayerIndex + " clicked : SHOW";
            Debugger.text =  PlayersList[PlayerIndex-1].GetComponent<Player>().name + " clicked : SHOW";

            if (PlayerIndex < PlayersList.Count)
            {
                PlayerIndex++;
            }
            else
            {
                PlayerIndex = 1;
            }
            //TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            TurnIndicator.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " is playing...";
            StartGame();
        }

        public void Bet(bool IsPlayer)
        {
            if(IsPlayer==true)
            {
                //Debugger.text = "Player " + PlayerIndex + " Betted " + BettingSlider.value + " Chips";
                Debugger.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " Betted " + BettingSlider.value + " Chips";

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
                //Debugger.text = "Player " + PlayerIndex + " clicked : BET";
                 Debugger.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " clicked : BET";

            }

            if (PlayerIndex < PlayersList.Count)
            {
                PlayerIndex++;
            }
            else
            {
                PlayerIndex = 1;
            }
            //TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            TurnIndicator.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " is playing...";
            StartGame();
        }

        public void Fold(bool IsPlayer)
        {
            if(IsPlayer==true)
            {
                PrimaryPlayerDead = true;
            }

            //Debugger.text = "Player " + PlayerIndex + " clicked : FOLD";
            Debugger.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " clicked : FOLD";

            Destroy(PlayersList[PlayerIndex - 1].gameObject);
            PlayersList.RemoveAt(PlayerIndex - 1);

            //for (int i = (PlayerIndex - 1); i < PlayersList.Count; i++)
           // {
               // PlayersList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Player " + (i + 1);
           // }

            if (PlayerIndex == PlayersList.Count + 1)
            {
                PlayerIndex = 1;
            }

            //TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            TurnIndicator.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " is playing...";
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
              
                    Utils.DoActionAfterSecondsAsync(StartPlayersTurn, WaitingTime);
                }
                else  //We are not dead.
                {
                    if (PlayerIndex != 1)  //If index is not 1 , Go On.
                    {
                  
                        Utils.DoActionAfterSecondsAsync(StartPlayersTurn, WaitingTime);
                    }
                    else //else wait for Main Player to do action.
                    {
                        //Check if Chips are less than Minimum Bid.

                        if(MainPlayer.GetComponent<Player>().coin < MinimumBettingValue)  //Auto Show Cards.
                        {
                            ChangeButtonState();
                            Utils.DoActionAfterSecondsAsync(Show, WaitingTime); // Auto SHOW.
                        }
                        else   
                        {
                            //Let Player Play.

                            //Wait for 45 seconds, If Player not played, Auto Fold.
                            
                            StartCoroutine(Wait45Seconds( (MainPlayerTimeoutIndex++) ));
                          
                        }
                    }
                    
                }
            }
            else  // Some Player Won...
            {
                //Debugger.text = "Yeah !! Player " + PlayerIndex + " won !";
                Debugger.text = "Yeah! Congo... " + PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " you WON !";
            }

        }

        IEnumerator Wait45Seconds(int Index)
        {
            int TempIndex = Index;   // Index to stop Mismatching 45 seconds of Previous Turn and Current Turn.
            yield return new WaitForSeconds(45f);
            if(PlayerIndex==1 && TempIndex==MainPlayerTimeoutIndex)
            {
                Fold(true);  //Fold is exceed, 45 seconds.
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
                    if(MainPlayer.GetComponent<Player>().coin > MinimumBettingValue)    //If Coins greter than beeting value
                    {
                        Interactable = true;
                    }
                    else
                    {
                        Interactable = false;
                    }
                    
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
