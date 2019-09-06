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
        public GameObject playerPrefab2;
        public Transform[] InstantiatePosition;
        public static int numberOfPlayer;

        public List<GameObject> ObjectsToDisable;
        
        public List<GameObject> PlayersList;
        public List<GameObject> TopRankers;

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
        public int MinimumBettingValue;
        public float WaitingTime = 5f;   //Waiting Time for all the actions.
        public Image TimerUI;

        //--
        public TMP_Text Greeting;
        public TMP_Text WinnerText;
        //--

        private int MainPlayerTimeoutIndex = 0;  // Index to stop Mismatching 45 seconds of Previous Turn and Current Turn.
        private float SliderFloorFunction;

        private int RoundsCompleted = 0;
        private int RoundsCompletedWithTwoPlayers = 0;
        private bool ShowClicked = false;

        public Animator[] cardanim;

        private void Start()
        {
            WebRequestManager.HttpGetPlayerData((List<GameNameSpace.Player> NewPlayerList) =>
            {
                numberOfPlayer = NewPlayerList.Count + 1;
                SecondStart();
            });

        }

        private float TimeTakeToComplete = 45f;
        private float CurrentTime = 0f;
        private bool TimerOn = false;

        private void Update()
        {
            if(TimerOn==true)
            {
                CurrentTime += Time.deltaTime;
                TimerUI.fillAmount = (CurrentTime / TimeTakeToComplete);
            }
            
        }

        private void SecondStart()
        {
            PlayerIndex = Random.Range(1, numberOfPlayer + 1);
            CardsManager.instance.MakeDatabase();
            CreatePlayers(numberOfPlayer);
        }

        private void ThirdStart()
        {
            TurnIndicator.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " is playing...";

            MinimumBettingValue = GameInstance.new_instance.MinimumBettingValue;


            ChangeSelectionUI();
            ChangeButtonState();
            RefreshSlider();
            RefreshPotText();

            //Ranking Happens Here.
            ReviewCards();
            WinnerText.text = "Winner is : " + TopRankers[0].GetComponent<Player>().name;

            Utils.DoActionAfterSecondsAsync(StartGame, 1f);
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
                            CreatePlayer(FB_Handler.instance.SavedUsername,5000, 1000, FB_Handler.instance.SavedProfile, i);
                            Greeting.text = "Welcome Back, " + FB_Handler.instance.SavedUsername;
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
            GameObject player;

            if (playernumber ==2 || playernumber ==3)
            {
                 player = Instantiate(playerPrefab2, InstantiatePosition[playernumber - 1]);
            }
            else
            {
                 player = Instantiate(playerPrefab, InstantiatePosition[playernumber - 1]);
            }

             PlayersList.Add(player);
            Player playerScript = player.GetComponent<Player>();


            playerScript.PLayerDefaultNumber = playernumber;
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

                  //  playerScript.PopulateCards();
                }
            }
            else   //For Other Players
            {
                if (playerScript != null)
                {
                    playerScript.PopulateData();
                   //playerScript.PopulateCards();
                }
            }
        }

        public void cardspopulate()
        {
            Player playerScript = PlayersList[0].GetComponent<Player>();
            cardanim =playerScript.GetComponentsInChildren<Animator>();
            playerScript.PopulateCards();
            cardanim[0].Play("CardFlipAnim");
        }


        public void Show()
        {
            ShowClicked = true;

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
                Debugger.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " Betted " + SliderFloorFunction + " Chips";

                MainPlayer.GetComponent<Player>().coin -= (int)SliderFloorFunction;
                TotalPot += (int)SliderFloorFunction;
                MinimumBettingValue = (int)SliderFloorFunction;
                //Refresh Data after some Maths..
                MainPlayer.GetComponent<Player>().RefreshData();
                RefreshSlider();
                RefreshChipsText();
                RefreshPotText();
            }
            else
            {
                 int multiple = Random.Range(1, 4);
                 int temp = MinimumBettingValue * multiple;

                 MinimumBettingValue = temp;
                 Debugger.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " Betted " + temp + " Chips";

                 PlayersList[PlayerIndex - 1].GetComponent<Player>().coin -= temp;
                 PlayersList[PlayerIndex - 1].GetComponent<Player>().chipsText.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().coin.ToString();
                 TotalPot += temp;
                 RefreshPotText();
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


            for (int i = 0; i < TopRankers.Count; i++)
            {
                if (TopRankers[i].GetComponent<Player>().PLayerDefaultNumber == PlayerIndex)
                {
                    TopRankers.RemoveAt(i);
                }
            }

            Destroy(PlayersList[PlayerIndex - 1].gameObject);
            PlayersList.RemoveAt(PlayerIndex - 1);

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
            CurrentTime = 0f;
            TimerUI.fillAmount = 0f;
            TimerOn = false;

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
                            CurrentTime = 0f;
                            TimerUI.fillAmount = 0f;
                            TimerOn = true;
                            MainPlayerTimeoutIndex++;
                            StartCoroutine(Wait45Seconds(MainPlayerTimeoutIndex));
                          
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
            if (PlayerIndex==1 && TempIndex==MainPlayerTimeoutIndex && PrimaryPlayerDead==false)
            {
                Fold(true);  //Fold is exceed, 45 seconds.
            }
        }

        private void StartPlayersTurn() //AI ALL DOWN.
        {
            if(PlayerIndex==PlayersList.Count)  //Counts Round Completed.
            {
                RoundsCompleted++;
                Debug.Log(RoundsCompleted + "Round Completed");
                if(PlayersList.Count==2)
                {
                    RoundsCompletedWithTwoPlayers++;
                }
            }


            //If MainPLayer is Winner.
            if (TopRankers[0].GetComponent<Player>().PLayerDefaultNumber == 1)
            {
                if (PlayersList.Count == 2) //Only Two Players are there
                {
                    //If MainPlayer is Playing...
                    if(PlayerIndex==1)
                    {
                        //He can do anything.
                    }
                    else  //If BOT is Playing...
                    {
                        //CAN "FOLD" AFTER 2 ROUNDS.
                        //CAN "SHOW" AFTER 2 ROUNDS.
                       
                        if(RoundsCompletedWithTwoPlayers>=2)
                        {
                            int k = Random.Range(1, 4);
                            switch(k)
                            {
                                case 1: Bet(false);
                                    break;

                                case 2:  Show();
                                    break;

                                case 3: Fold(false);
                                    break;
                            }
                        }
                        else
                        {
                            Bet(false);
                        }

                    }
                }
                else  //More than two Players are there.
                {
                    //If MainPlayer is Playing...
                    if (PlayerIndex == 1)
                    {
                        //He can do anything.
                    }
                    else  //If BOT is Playing...
                    {
                        //CAN "SHOW" AFTER 2 ROUNDS.
                        //WILL NOT FOLD EVER.
                        if (RoundsCompleted >= 2)
                        {
                            int k = Random.Range(1, 3);
                            switch (k)
                            {
                                case 1: Bet(false);
                                    break;

                                case 2: Show();
                                    break;
                            }
                        }
                        else
                        {
                            Bet(false);
                        }
                    }
                }
            }
            //If MainPLayer is 2nd Winner.
            else if (TopRankers[1].GetComponent<Player>().PLayerDefaultNumber == 1)
            {
                if (PlayersList.Count == 2) 
                {
                    //If MainPlayer is Playing...
                    if (PlayerIndex == 1)
                    {
                        //He can do anything.
                    }
                    else  //If BOT is Playing...
                    {
                        //CAN "SHOW" AFTER 2 ROUNDS.
                        //WILL NOT FOLD EVER.
                        if (RoundsCompletedWithTwoPlayers >= 2)
                        {
                            int k = Random.Range(1, 3);
                            switch (k)
                            {
                                case 1:
                                    Bet(false);
                                    break;

                                case 2:
                                    Show();
                                    break;
                            }
                        }
                        else
                        {
                            Bet(false);
                        }
                    }
                }
                else
                {
                    //If MainPlayer is Playing...
                    if (PlayerIndex == 1)
                    {
                        //He can do anything.
                    }
                    else  //If BOT is Playing...
                    {
                        //CAN "SHOW" AFTER 2 ROUNDS.
                        //WILL NOT FOLD EVER.
                        if (RoundsCompleted >= 2)
                        {
                            int k = Random.Range(1, 3);
                            switch (k)
                            {
                                case 1:
                                    Bet(false);
                                    break;

                                case 2:
                                    Show();
                                    break;
                            }
                        }
                        else
                        {
                            Bet(false);
                        }
                    }
                }
            }
            //If only Bots are at Top two positions.
            else
            {
                if (PlayersList.Count == 2) //Only Two Players are there
                {
                    //CAN DO WHATEVER THEY WANT. BET / FOLD.
                    //CAN SHOW AFTER TWO ROUNDS.
                    if (RoundsCompletedWithTwoPlayers >= 2)
                    {
                        Show();
                    }
                    else
                    {
                        int k = Random.Range(1, 3);
                        switch (k)
                        {
                            case 1:
                                Bet(false);
                                break;

                            case 2:
                                Fold(false);
                                break;
                        }
                    }
                }
                else  //More than two Players are there.
                {
                    //CAN NOT FOLD.
                    //CAN SHOW AFTER TWO ROUNDS.
                    if (RoundsCompleted >= 2)
                    {
                        Show();
                    }
                    else
                    {
                        Bet(false);
                    }
                }
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
                    if(MainPlayer.GetComponent<Player>().coin >= MinimumBettingValue)    //If Coins greter than beeting value
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
            float value = BettingSlider.value / MinimumBettingValue;
            SliderFloorFunction = Mathf.Floor(value) * MinimumBettingValue;
            BettingValueText.text = SliderFloorFunction.ToString();
        }

        public void AddBetButton()
        {
            if(SliderFloorFunction<BettingSlider.maxValue)
            {
                SliderFloorFunction += MinimumBettingValue;
                BettingSlider.value = SliderFloorFunction;
                BettingValueText.text = SliderFloorFunction.ToString();
            }

        }
        public void SubstractBetButton()
        {
            if (SliderFloorFunction > BettingSlider.minValue)
            {
                SliderFloorFunction -= MinimumBettingValue;
                BettingSlider.value = SliderFloorFunction;
                BettingValueText.text = SliderFloorFunction.ToString();
            }
        }

        public void RefreshSlider()
        {
            BettingSlider.minValue = MinimumBettingValue;
            BettingSlider.value = SliderFloorFunction;
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


        #region  Card_Algorithm

        public void ReviewCards()  //MAIN FUNCTION
        {
            for (int i = 1; i <= numberOfPlayer; i++)
            {
                CheckSequence(i);
                CheckSuit(i);
                CheckRank(i);
                PlayersList[i - 1].GetComponent<Player>().DeckLayer = AssignDeckLayer(i);  //RETURNED VALUE OF DECK LAYER.
                PlayersList[i - 1].GetComponent<Player>().DeckSubLayer = AssignDeckSubLayer(i, PlayersList[i - 1].GetComponent<Player>().DeckLayer);  //RETURNED VALUE OF DECK SUB-LAYER.
                RankPlayers(i);  //RANKING HAPPENS HERE.
            }
        }

        public void CheckSequence(int ForWhichPlayer) //Check Sequence of Cards in a Deck. Will return "True" if in sequence, else "False".
        {
            SortInDesending(ForWhichPlayer);  // NEED TO BE SORTED IN DESENDING ORDER.

            if ((PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank - 1)
                && (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank - 1))
            {
                //IN CONSECUTIVE DESENDING SEQUENCE
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().Ordered = true;
            }
            else
            {
                //NOT IN SEQUENCE
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().Ordered = false;
            }

        }

        public void SortInDesending(int ForWhichPlayer)
        {
            if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank <= PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank)
            {
                int temp = PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank;
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank = PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank;
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank = temp;
            }

            if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank <= PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank)
            {
                int temp = PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank;
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank = PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank;
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank = temp;
            }

            if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank <= PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank)
            {
                int temp = PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank;
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank = PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank;
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank = temp;
            }
        }

        public void CheckRank(int ForWhichPlayer)  //Tells Behaviour of Deck with respect to "RANK".
        {
            if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank &&
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank)
            {
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameRank = 3;
            }
            else
            if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank ||
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank ||
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank)
            {
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameRank = 2;
            }
            else
            {
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameRank = 0;
            }
        }

        public void CheckSuit(int ForWhichPlayer)  //Tells Behaviour of Deck with respect to "Suit".
        {
            if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Suit == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Suit &&
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Suit == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Suit)
            {
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameSuit = 3;
            }
            else
            if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Suit == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Suit ||
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Suit == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Suit ||
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Suit == PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Suit)
            {
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameSuit = 2;
            }
            else
            {
                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameSuit = 0;
            }
        }





        public int AssignDeckLayer(int ForWhichPlayer)  //Gives integer of Layer it belongs. From 1 to 6.
        {


            if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameRank == 3) //Layer 1 //Three Cards Of Same Rank.
            {
                return 1;
            }
            else if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().Ordered == true && PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameSuit == 3) //Layer 2 // Consecutive Cards && Same Suit.
            {
                return 2;
            }
            else if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().Ordered == true && PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameSuit != 3) //Layer 3 // Consecutive Cards && Diffrent Suit.
            {
                return 3;
            }
            else if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().Ordered == false && PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameSuit == 3) //Layer 4 // Non-Consecutive Cards && Same Suit.
            {
                return 4;
            }
            else if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().NoOfSameRank == 2) //Layer 5 // Two Cards of Same Rank.
            {
                return 5;
            }
            else //Layer 6 //
            {
                return 6;
            }

            //return 0; // Never going to happen.
        }


        public int AssignDeckSubLayer(int ForWhichPlayer, int LayerNumber)  //
        {
            switch (LayerNumber)
            {
                case 1:
                    if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank == 1)
                    {
                        return 1;  //AAA as 1.
                    }
                    else
                    {
                        return (15 - PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank); //KKK(13) as 2, QQQ(12) as 3 , .... 222(2) as 13.
                    }
                case 2:
                    if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank == 13 &&
                                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank == 12 &&
                                        PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank == 1)
                    {
                        return 1;  // KQA
                    }
                    else if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank == 3 &&
                                    PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank == 2 &&
                                        PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank == 1)
                    {
                        return 2;  //32A
                    }
                    else
                    {
                        return (16 - PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank); //13,12,11 -  12,11,10 - 10,9,8
                    }
                case 3:
                    if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank == 13 &&
                                PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank == 12 &&
                                        PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank == 1)
                    {
                        return 1;  // KQA
                    }
                    else if (PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank == 3 &&
                                    PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[1].Rank == 2 &&
                                        PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[2].Rank == 1)
                    {
                        return 2;  //32A
                    }
                    else
                    {
                        return (16 - PlayersList[ForWhichPlayer - 1].GetComponent<Player>().cardList[0].Rank); //13,12,11 -  12,11,10 - 10,9,8
                    }
                default:  //FOR 4,5,6 //THIS WILL BE CHECKED WHILE RANKING.
                    return 0;
            }
        }

        public void RankPlayers(int ForWhichPlayer)  //RANKING ALGORITHM.
        {
            int CurrentPosition = 0;
            TopRankers.Insert(0, PlayersList[ForWhichPlayer - 1]);

            //------------------Change Every A card index to 14--------------------------
            for(int i=0;i<3;i++)
            {
                if(TopRankers[0].GetComponent<Player>().cardList[i].Rank == 1)
                {
                    TopRankers[0].GetComponent<Player>().cardList[i].Rank = 14;
                }
            }
           


            void Swap()
            {
                GameObject temp = TopRankers[CurrentPosition+1];
                TopRankers.RemoveAt(CurrentPosition + 1);
                TopRankers.Insert(CurrentPosition, temp);
                CurrentPosition += 1;
            }

            for (int i = CurrentPosition; i < (TopRankers.Count - 1); i++)
            {
                if (TopRankers[CurrentPosition].GetComponent<Player>().DeckLayer > TopRankers[CurrentPosition + 1].GetComponent<Player>().DeckLayer)   //SORT WITH HELP OF LAYERS INT.
                {
                    Swap();   //SWAP on basis of Deck_Layer
                }
                else
                {
                    break;
                }
            }

            for (int i = CurrentPosition; i < (TopRankers.Count - 1); i++)
            {
                if (TopRankers[CurrentPosition].GetComponent<Player>().DeckLayer == TopRankers[CurrentPosition + 1].GetComponent<Player>().DeckLayer)
                {
                    if (TopRankers[CurrentPosition].GetComponent<Player>().DeckLayer <= 3)
                    {
                        //It is from Deck Layer 1, 2, 3
                        if (TopRankers[CurrentPosition].GetComponent<Player>().DeckLayer > TopRankers[CurrentPosition + 1].GetComponent<Player>().DeckLayer)  //If Smaller , Insert Above
                        {
                            GameObject temp = TopRankers[CurrentPosition];
                            TopRankers.Insert(CurrentPosition, TopRankers[CurrentPosition + 1]);
                            TopRankers.Insert(CurrentPosition + 1, temp);
                            CurrentPosition += 1;
                        }
                    }
                    else if (TopRankers[CurrentPosition].GetComponent<Player>().DeckLayer == 5) //THEY ARE 5. First check highest Pair-Cards, 
                    {
                        if (TopRankers[CurrentPosition].GetComponent<Player>().cardList[0].Rank == TopRankers[CurrentPosition].GetComponent<Player>().cardList[1].Rank)
                        {
                            //First 2 Cards are same.
                            if (TopRankers[CurrentPosition].GetComponent<Player>().cardList[0].Rank < TopRankers[CurrentPosition + 1].GetComponent<Player>().cardList[0].Rank) //IF GREATER
                            {
                                //swap
                                Swap();
                            }
                        }
                        else
                        {
                            //Last 2 Cards are same.
                            if (TopRankers[CurrentPosition].GetComponent<Player>().cardList[1].Rank < TopRankers[CurrentPosition + 1].GetComponent<Player>().cardList[1].Rank) //IF GREATER
                            {
                                //swap
                                Swap();
                            }
                        }
                    }
                    else //THEY ARE 4,6
                    {
                        if (TopRankers[CurrentPosition].GetComponent<Player>().cardList[0].Rank < TopRankers[CurrentPosition + 1].GetComponent<Player>().cardList[0].Rank) //IF GREATER
                        {
                            //swap
                            Swap();
                        }
                        else if (TopRankers[CurrentPosition].GetComponent<Player>().cardList[0].Rank == TopRankers[CurrentPosition + 1].GetComponent<Player>().cardList[0].Rank) //IF EQUAL
                        {
                            //check 2nd card
                            if (TopRankers[CurrentPosition].GetComponent<Player>().cardList[1].Rank < TopRankers[CurrentPosition + 1].GetComponent<Player>().cardList[1].Rank) //IF GREATER
                            {
                                //swap
                                Swap();
                            }
                            else if (TopRankers[CurrentPosition].GetComponent<Player>().cardList[1].Rank == TopRankers[CurrentPosition + 1].GetComponent<Player>().cardList[1].Rank) //IF EQUAL
                            {
                                //check 3nd card
                                if (TopRankers[CurrentPosition].GetComponent<Player>().cardList[2].Rank < TopRankers[CurrentPosition + 1].GetComponent<Player>().cardList[2].Rank) //IF GREATER
                                {
                                    //swap
                                    Swap();
                                }
                            }
                            else {/*DO NOTHING*/  break; }
                        }
                        else {/*DO NOTHING*/  break; }
                    }

                }
                else
                {
                    break;
                }
            }
        }

        #endregion  


    }
}
