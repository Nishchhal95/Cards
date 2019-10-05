using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace GameNameSpace
{
    public class _GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject playerPrefab2;
        public Transform[] InstantiatePosition;
        public static int numberOfPlayer = 5;

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

        public Button PlusButton;
        public Button MinusButton;

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
       // public TMP_Text WinnerText;
        //--
        public Sprite ChaalSprite;
        public Button BetButton;

        private int MainPlayerTimeoutIndex = 0;  // Index to stop Mismatching 45 seconds of Previous Turn and Current Turn.

        private int RoundsCompleted = -1;  //Keep this -1.

        private bool ShowClicked = false;
        private int NumberOfPlayerShowed = 0;

        private int RandomPlayerStartAt = 0;
        private int BetValue = 0;  // Value Player is Betting with.

        private int MinBetValueForUser = 0;  // Value Player is Betting with.
        private int MaxBetValueForUser = 0;  // Value Player is Betting with.

        public Animator[] cardanim;
        private bool UserFirstTurn = false;

        public GameObject GameWonPanel;
        public TMP_Text FinalWinnerText;
        public TMP_Text NextGameText;

        public Button SeeCardsButton;

        private int ErrorRetryCount = 5;
        public GameObject ErrorConnectingPanel;

        private string json;
        public Sprite BotSprite;

        public GameObject LoadingBarPanel;

        public Player TopPlayerInChart;
        public Player WinPlayerForChart;

        private void Start()
        {
            RoundsCompleted = -1;
            StartCoroutine(RequestData());
        }

        IEnumerator RequestData()
        {
            /* WebRequestManager.HttpGetPlayerData((List<GameNameSpace.Player> NewPlayerList) =>
             {
                 numberOfPlayer = NewPlayerList.Count + 1;
                 SecondStart();
             }, () => {
                 ErrorConnecting();
             });
             */

            int MinEarlyBet = GameInstance.new_instance.MinimumBettingValue;  


            RefilPlayerMessage refilPlayerMessage = new RefilPlayerMessage
            {
                minimumBet = MinEarlyBet,
                noOfPlayers = numberOfPlayer - 1
            };

            yield return new WaitForSeconds(0.1f);

            json = JsonConvert.SerializeObject(refilPlayerMessage);

            yield return new WaitForSeconds(0.1f);

            SecondStart();
        }


        private void ErrorConnecting()
        {
            if(ErrorRetryCount>0)
            {
                RequestData();
                ErrorRetryCount--;
            }
            else
            {
                ErrorConnectingPanel.SetActive(true);
            }
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
            RandomPlayerStartAt = PlayerIndex;
            if(PlayerIndex==1)
            {
                UserFirstTurn = true;
            }

            CardsManager.instance.MakeDatabase();
            CreatePlayers(numberOfPlayer);
        }

        private void CreatePlayers(int playerNumbers)
        {
            WebRequestManager.HttpRefilsPlayers(json, (List<PlayerData> NewPlayerList) =>
            {
                StartCoroutine(SetPlayers(NewPlayerList, playerNumbers));
            });
        }

        IEnumerator SetPlayers(List<PlayerData> NewPlayerList, int playerNumbers)
        {
            for (int i = 1; i <= playerNumbers; i++)
            {
                if (i == 1)
                {
                    if (FB_Handler.instance.SavedProfile != null)
                    {
                        CreatePlayer(FB_Handler.instance.SavedUsername, MainMenu.UserCurrentChips, 1000, FB_Handler.instance.SavedProfile, i);
                        Greeting.text = "Welcome Back, " + FB_Handler.instance.SavedUsername;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(1f);  //THIS IS VERY IMPORTANT TO HOLD FOR LOOP FOR WHILE.
                    if(GameInstance.new_instance.GetPlayersFromServer==true)
                    {
                        //StartCoroutine(GetTexture(NewPlayerList, i));  // i passes 2, 3, till number of players

                        UnityWebRequest www = UnityWebRequestTexture.GetTexture(NewPlayerList[i - 2].pic);

                        yield return www.SendWebRequest();

                        if (www.isNetworkError)
                        {
                            Debug.Log(www.error + " : " + NewPlayerList[i - 2].pic);
                            UnityWebRequest defWWW = UnityWebRequestTexture.GetTexture("https://mir-s3-cdn-cf.behance.net/project_modules/1400_opt_1/dde50c85963513.5d8b73a656202.jpg");

                            yield return defWWW.SendWebRequest();

                            Debug.Log("Image Load Succefully " + i);
                            Texture myTexture = ((DownloadHandlerTexture)defWWW.downloadHandler).texture;
                            BotSprite = Sprite.Create((Texture2D)myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
                            CreatePlayer(NewPlayerList[i - 2].name, int.Parse(NewPlayerList[i - 2].coins), 100, BotSprite, i);

                            if (i == playerNumbers)
                            {
                                ThirdStart();
                            }
                            //GetTexture(P, index);
                        }
                        else
                        {
                            Debug.Log("Image Load Succefully " + i);
                            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                            BotSprite = Sprite.Create((Texture2D)myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
                            CreatePlayer(NewPlayerList[i - 2].name, int.Parse(NewPlayerList[i - 2].coins), 100, BotSprite, i);

                            if (i == playerNumbers)
                            {
                                ThirdStart();
                            }
                        }
                    }
                    else
                    {
                        CreatePlayer(GameInstance.new_instance.ServerPlayersNames[i - 2], GameInstance.new_instance.ServerPlayersCoins[i - 2], 100, GameInstance.new_instance.ServerPlayersImages[i - 2], i);

                        if (i==playerNumbers)
                        {
                            ThirdStart();
                        }
                    }
                }
            }
        }


        private void ThirdStart()
        {
            for(int i=1;i<PlayersList.Count;i++)
            {
                GameInstance.new_instance.ServerPlayersImages.Add(PlayersList[i].GetComponent<Player>().profileImage.sprite);
                GameInstance.new_instance.ServerPlayersCoins.Add(PlayersList[i].GetComponent<Player>().coin);
                GameInstance.new_instance.ServerPlayersNames.Add(PlayersList[i].GetComponent<Player>().name);
            }
           

            LoadingBarPanel.GetComponent<LoadingScript>().SetDisable();

            TurnIndicator.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " is playing...";

            MinimumBettingValue = GameInstance.new_instance.MinimumBettingValue;
 
            BetValue = MinimumBettingValue;

            BetUpdate();
            ChangeSelectionUI();

            if(UserFirstTurn==false)
            {
                ChangeButtonState();
            }
           

            RefreshPotText();

            //Ranking Happens Here.
            ReviewCards();

            //Changing List to Let User Win.

           
            if(GameInstance.WinCounter==GameInstance.MakePlayerWin)
            {
                HandleUserWin(true);
                GameInstance.MakePlayerWin = Random.Range(0, 5);
            }
            else
            {
                HandleUserWin(false);
            }

            GameInstance.WinCounter++;
            if(GameInstance.WinCounter>4)
            {
               GameInstance.WinCounter = 0;
            }

            // WinnerText.text = "Winner is : " + TopRankers[0].GetComponent<Player>().name;

            Utils_X.DoActionAfterSecondsAsync(StartGame, 1f);
        }

      

        private void HandleUserWin(bool LetUserWin)
        {
            if(LetUserWin == true) //User should win.
            {
                if(TopRankers[0].GetComponent<Player>().PlayerDefaultNumber!=1)  //User is not winner. Make him winner.
                {
                    for(int i=0; i<TopRankers.Count; i++)
                    {
                        if (TopRankers[i].GetComponent<Player>().PlayerDefaultNumber == 1)
                        {
                            GameObject B = TopRankers[0]; //Get Topper Bot.
                            //Remove User from existing position and Add to Top position.
                            GameObject P = TopRankers[i];
                            TopRankers.Remove(P);
                            TopRankers.Insert(0, P);
                            //Remove Topper and Put to Early User Position.
                            TopRankers.Remove(B);
                            TopRankers.Insert(i, B);
                            //-----------------------------------------------
                            //Swap their Cards.
                            List<CardsManager.Card> TempCards = TopRankers[0].GetComponent<Player>().cardList;
                            TopRankers[0].GetComponent<Player>().cardList = TopRankers[i].GetComponent<Player>().cardList;
                            TopRankers[i].GetComponent<Player>().cardList = TempCards;
                            //-----
                            break;
                        }
                    }
                }
            }
            else //User should lose.
            {
                if (TopRankers[0].GetComponent<Player>().PlayerDefaultNumber == 1)  //User is winner. Make him loser.
                {
                    int i = Random.Range(1, TopRankers.Count);

                    GameObject P= TopRankers[0]; //Get User.

                    //Remove Bot from existing position and Add to Top position.
                    GameObject B = TopRankers[i];
                    TopRankers.Remove(B);
                    TopRankers.Insert(0, B);
                    //Remove User and Put to Early User Position.
                    TopRankers.Remove(P);
                    TopRankers.Insert(i, P);
                    //-----------------------------------------------
                    //Swap their Cards.
                    List<CardsManager.Card> TempCards = TopRankers[0].GetComponent<Player>().cardList;
                    TopRankers[0].GetComponent<Player>().cardList = TopRankers[i].GetComponent<Player>().cardList;
                    TopRankers[i].GetComponent<Player>().cardList = TempCards;
                }
                     
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Player");
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

            if (playernumber == 1)
            {
                playerScript.StatusText.enabled = false;
            }
            playerScript.PlayerDefaultNumber = playernumber;
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
                    //  MainPlayerChips.text = playerScript.coin.ToString();
                    MainPlayerChips.text = MainMenu.UserCurrentChips.ToString();
                    MainPlayerImage.sprite = playerScript.playerSprite;

                    playerScript.CardSet.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);

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

            Debug.Log("Player Added with Default Number " + playernumber);
        }

        public void cardspopulate()
        {
            Player playerScript = PlayersList[0].GetComponent<Player>();
            playerScript.StatusSeen = true;
            playerScript.StatusText.text = "SEEN";
            BetButton.GetComponent<Image>().sprite = ChaalSprite;
            cardanim =playerScript.GetComponentsInChildren<Animator>();
            playerScript.PopulateCards();
            cardanim[0].Play("CardFlipAnim");

            if(CanBet()==true)
            {
                BetUpdate();
            }
          
        }

        public void WinningCardAnimation()
        {
            foreach(GameObject P in PlayersList)
            {
                if (P.GetComponent<Player>().PlayerDefaultNumber != 1)
                {
                    P.GetComponent<Player>().GetComponentsInChildren<Animator>()[0].Play("CardFlipAnim");
                    P.GetComponent<Player>().PopulateCards();
                }
                else
                {
                    if(ShowClicked==false)
                    {
                        MainPlayer.GetComponent<Player>().GetComponentsInChildren<Animator>()[0].Play("CardFlipAnim");
                        MainPlayer.GetComponent<Player>().PopulateCards();
                    }
                }
            }
        }


        public void Show()
        {
            ShowClicked = true;
            NumberOfPlayerShowed++;
            MainPlayerTimeoutIndex++;

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
            MainPlayerTimeoutIndex++;
            if (IsPlayer==true)
            {
                //Debugger.text = "Player " + PlayerIndex + " Betted " + BettingSlider.value + " Chips";
                Debugger.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " Betted " + BetValue + " Chips";
                MainPlayer.GetComponent<Player>().coin -= BetValue;
                MainMenu.UserCurrentChips -= BetValue;
                WebRequestManager.HttpGetReductedCoin(FB_Handler.instance.SavedEmail, BetValue.ToString(), () =>
                {
                    Debug.Log("BET VALUE REDUCTED");
                });
                TotalPot += BetValue;
                MinimumBettingValue = BetValue;
                //Refresh Data after some Maths..
                MainPlayer.GetComponent<Player>().RefreshData();
                RefreshChipsText();
                RefreshPotText();
            }
            else
            {
                MinimumBettingValue = BetValue;
                Debugger.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " Betted " + BetValue + " Chips";

                PlayersList[PlayerIndex - 1].GetComponent<Player>().coin -= BetValue;
                PlayersList[PlayerIndex - 1].GetComponent<Player>().chipsText.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().coin.ToString();
                TotalPot += BetValue;
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

         
            TurnIndicator.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " is playing...";
            StartGame();
        }

        public void Fold(bool IsPlayer)
        {
            MainPlayerTimeoutIndex++;

            if (IsPlayer==true)
            {
                PrimaryPlayerDead = true;
                SeeCardsButton.gameObject.SetActive(false);
            }

            //Debugger.text = "Player " + PlayerIndex + " clicked : FOLD";
            Debugger.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " clicked : FOLD";


          

           /* for (int i = 0; i<TopRankers.Count; i++)
            {
                if (TopRankers[i].GetComponent<Player>().PLayerDefaultNumber==PlayerIndex)
                {
                    TopRankers.RemoveAt(i);
                    break;
                }
            }*/

            GameObject G = PlayersList[PlayerIndex - 1];

            PlayersList.Remove(G);
            TopRankers.Remove(G);

            //Destroy(G);


            if (PlayerIndex == PlayersList.Count + 1)
            {
                PlayerIndex = 1;
            }

            //TurnIndicator.text = "Player " + PlayerIndex + " is playing...";
            TurnIndicator.text = PlayersList[PlayerIndex - 1].GetComponent<Player>().name + " is playing...";
            StartGame();
        }

        public void FoldByBot()
        {
            Fold(false);
        }

        private void StartGame()
        {
            CurrentTime = 0f;
            TimerUI.fillAmount = 0f;
            TimerOn = false;

            if(UserFirstTurn==false)
            {
                ChangeButtonState();
            }
            else
            {
                BettingSlider.interactable = false;
                PlusButton.interactable = false;
                MinusButton.interactable = false;
                UserFirstTurn = false;
            }
            


            ChangeSelectionUI();
            BetUpdate();


            if (PlayersList.Count!=1) //Game move forward, only if there are more than one player.
            {
                if (PrimaryPlayerDead == true)  //We are dead, Go On in Loop.
                {
                    RandomToSeen(PlayerIndex);
                    
                    if(CanBet()==false)  //Auto Show/Fold Cards.
                    {
                        if(TopRankers[0].GetComponent<Player>().PlayerDefaultNumber == PlayerIndex) //Is going to be winner.
                        {
                            //It will Show Only.
                            Utils_X.DoActionAfterSecondsAsync(Show, WaitingTime); // Auto SHOW.
                        }
                        else
                        {
                            int k = Random.Range(1, 3);
                            if (k == 1)
                            {
                                Utils_X.DoActionAfterSecondsAsync(Show, WaitingTime); // Auto SHOW.
                            }
                            else //k==2
                            {
                                Utils_X.DoActionAfterSecondsAsync(FoldByBot, WaitingTime); // Auto FOLD.

                            }
                        }
                    }
                    else
                    {
                        Utils_X.DoActionAfterSecondsAsync(StartPlayersTurn, WaitingTime);
                    }
                }
                else  //We are not dead.
                {
                    if (PlayerIndex != 1)  //If index is not 1 , Go On.
                    {
                        RandomToSeen(PlayerIndex);

                        if (CanBet()==false)  //Auto Show Cards.
                        {
                            Utils_X.DoActionAfterSecondsAsync(Show, WaitingTime); // Auto SHOW.
                        }
                        else
                        {
                            Utils_X.DoActionAfterSecondsAsync(StartPlayersTurn, WaitingTime);
                        }
                    }
                    else //else wait for Main Player to do action.
                    {
                        //Check if Chips are less than Minimum Bid.

                        if(CanBet() == false) //Here Just to know, Can Bet or Not. No need of Bet Value.
                        {
                            ChangeButtonState();
                            Utils_X.DoActionAfterSecondsAsync(Show, WaitingTime); // Auto SHOW.
                        }
                        else   
                        {
                            //Let Player Play.
                            BetUpdate();

                            if (ShowClicked==true)
                            {
                                for (int i = 0; i < ObjectsToDisable.Count; i++)
                                {
                                    if (ObjectsToDisable[i].GetComponent<Button>() == true)
                                    {
                                        ObjectsToDisable[i].GetComponent<Button>().interactable = false;
                                    }
                                    else
                                    {
                                        ObjectsToDisable[i].GetComponent<Slider>().interactable = false;
                                    }
                                }

                                if ((NumberOfPlayerShowed + 1) == PlayersList.Count)
                                {
                                    GameWon();
                                }
                                else
                                {
                                    Utils_X.DoActionAfterSecondsAsync(Show, WaitingTime); // Auto SHOW.
                                }
                            }
                            else
                            {
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
            }
            else  // Some Player Won...
            {
                GameWon();
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

        private bool CanBet()
        {
            bool AbleToBet = true;  //Keep this true.
            bool CurrentPlayerStatus;
            bool PreviousPlayerStatus;

            CurrentPlayerStatus = PlayersList[PlayerIndex - 1].GetComponent<Player>().StatusSeen;

            if (PlayerIndex==1)  //If bot is at player index 1.
            {
                PreviousPlayerStatus = PlayersList[PlayersList.Count-1].GetComponent<Player>().StatusSeen;
            }
            else
            {
                PreviousPlayerStatus = PlayersList[PlayerIndex - 2].GetComponent<Player>().StatusSeen;
            }

            if (CurrentPlayerStatus == false)
            {
                //If Player playing Blind.
                if (PreviousPlayerStatus == false)
                {  //If Player playing Before is Blind.

                    //Can Bet x2 OR Same of Minimum Bet
                    MaxBetValueForUser = MinimumBettingValue * 2;
                    MinBetValueForUser = MinimumBettingValue;

                    if (PlayersList[PlayerIndex - 1].GetComponent<Player>().coin >= (MinimumBettingValue * 2))  //He can Bet x2
                    {
                        int k = Random.Range(1, 5);  // 1 , 2 , 3 , 4
                        switch (k)
                        {
                            //Less Probablity of Betting x2.
                            case 1:
                                BetValue = MinimumBettingValue;
                                break;

                            case 2:
                                BetValue = MinimumBettingValue;
                                break;

                            case 3:
                                BetValue = MinimumBettingValue;
                                break;

                            case 4:
                                BetValue = MinimumBettingValue * 2;
                                break;
                        }
                    }
                    else if(PlayersList[PlayerIndex - 1].GetComponent<Player>().coin >= (MinimumBettingValue))//He can bet Same Only.
                    {
                        BetValue = MinimumBettingValue;
                    }
                    else
                    {
                        AbleToBet = false;
                    }
                }
                else
                {
                    //If Player playing Before is Seen.

                    //Can Bet x1/2 OR Same of Minimum Bet
                    MaxBetValueForUser = MinimumBettingValue;
                    MinBetValueForUser = MinimumBettingValue/2;

                    if (PlayersList[PlayerIndex - 1].GetComponent<Player>().coin >= MinimumBettingValue)  //He can Bet Same Or Half
                    {
                        int k = Random.Range(1, 3);  // 1 , 2 
                        switch (k)
                        {
                            //Less Probablity of Betting x2.
                            case 1:
                                BetValue = MinimumBettingValue;
                                break;

                            case 2:
                                BetValue = MinimumBettingValue/2;
                                break;
                        }
                    }
                    else if (PlayersList[PlayerIndex - 1].GetComponent<Player>().coin >= (MinimumBettingValue/2)) //He can bet Half Only.
                    {
                        BetValue = (MinimumBettingValue/2);
                    }
                    else
                    {
                        AbleToBet = false;
                    }
                }
            }
            else
            {
                //If Player playing Seen.
                if (PreviousPlayerStatus == false)
                {  //If Player playing Before is Blind.

                    //Can Bet x2 OR x4 of Minimum Bet
                    MaxBetValueForUser = MinimumBettingValue * 4;
                    MinBetValueForUser = MinimumBettingValue * 2;

                    if (PlayersList[PlayerIndex - 1].GetComponent<Player>().coin >= (MinimumBettingValue * 4))  //He can Bet x4 OR x2
                    {
                        int k = Random.Range(1, 5);  // 1 , 2 , 3 , 4
                        switch (k)
                        {
                            //Less Probablity of Betting x4.
                            case 1:
                                BetValue = MinimumBettingValue * 2;
                                break;

                            case 2:
                                BetValue = MinimumBettingValue * 2;
                                break;

                            case 3:
                                BetValue = MinimumBettingValue * 2;
                                break;

                            case 4:
                                BetValue = MinimumBettingValue * 4;
                                break;
                        }

                    }
                    else if (PlayersList[PlayerIndex - 1].GetComponent<Player>().coin >= (MinimumBettingValue * 2)) //He can Bet x2
                    {
                        BetValue = MinimumBettingValue * 2;
                    }
                    else
                    {
                        AbleToBet = false;
                    }

                }
                else
                {   //If Player playing Before is Seen.

                    //Can Bet x2 OR Same of Minimum Bet
                    MaxBetValueForUser = MinimumBettingValue * 2;
                    MinBetValueForUser = MinimumBettingValue;

                    if (PlayersList[PlayerIndex - 1].GetComponent<Player>().coin >= (MinimumBettingValue * 2))  //He can Bet x2 Or Same
                    {
                        int k = Random.Range(1, 5);  // 1 , 2 , 3 , 4
                        switch (k)
                        {
                            //Less Probablity of Betting x2.
                            case 1:
                                BetValue = MinimumBettingValue;
                                break;

                            case 2:
                                BetValue = MinimumBettingValue;
                                break;

                            case 3:
                                BetValue = MinimumBettingValue;
                                break;

                            case 4:
                                BetValue = MinimumBettingValue * 2;
                                break;
                        }
                    }
                    else if(PlayersList[PlayerIndex - 1].GetComponent<Player>().coin >= (MinimumBettingValue))//He can bet Same Only.
                    {
                        BetValue = MinimumBettingValue;
                    }
                    else
                    {
                        AbleToBet = false;
                    }
                }
            }

            return AbleToBet;

        }
      

        private void StartPlayersTurn() //AI ALL DOWN.
        {

           // BetUpdate();

            if(RandomPlayerStartAt>PlayersList.Count)
            {
                RandomPlayerStartAt = PlayersList.Count;
            }

            if (PlayerIndex==RandomPlayerStartAt)  //Counts Round Completed.
            {
                RoundsCompleted++;
                Debug.Log(RoundsCompleted + "Round Completed");
            }

            if(ShowClicked==false)
            {
                //If MainPLayer is Winner.
                if (TopRankers[0].GetComponent<Player>().PlayerDefaultNumber == 1)
                {
                    //CAN "FOLD" AFTER 2 ROUNDS.
                    //CAN "SHOW" AFTER 2 ROUNDS.
                    if (RoundsCompleted >= 3) //Three Round Completed. 
                    {
                        int k = Random.Range(1, 3);
                        switch (k)
                        {
                            case 1:
                                Show();
                                break;

                            case 2:
                                Fold(false);
                                break;
                        }
                    }
                    else if (RoundsCompleted == 2) //Two Round Completed. 
                    {
                        int k = Random.Range(1, 6);  // 1, 2, 3, 4, 5
                        switch (k)
                        {
                            //Probablity of Show and Fold will increase afetr 2 turns.
                            case 1:
                                Bet(false);
                                break;

                            case 2:
                                Show();
                                break;

                            case 3:
                                Fold(false);
                                break;

                            case 4:
                                Show();
                                break;

                            case 5:
                                Fold(false);
                                break;
                        }
                    }
                    else if (RoundsCompleted == 1)
                    {
                        int k = Random.Range(1, 4);  // 1, 2, 3
                        switch (k)
                        {
                            //Probablity of Show and Fold will increase afetr 2 turns.
                            case 1:
                                Bet(false);
                                break;

                            case 2:
                                Fold(false);
                                break;

                            case 3:
                                Fold(false);
                                break;
                        }
                    }
                    else
                    {
                        Bet(false);
                    }
                }
                else //IF Bot will be the winner.
                {
                    if(PrimaryPlayerDead==true)  //WILL NOT FOLD UNTIL, MAIN PLAYER FOLDS.
                    {
                        int k = Random.Range(1, 5); // 1, 2, 3, 4
                        switch (k)
                        {
                            //Show 1 out of 4.
                            case 1:
                                Fold(false);
                                break;

                            case 2:
                                Fold(false);
                                break;

                            case 3:
                                Show();
                                break;

                            case 4:
                                Fold(false);
                                break;
                        }
                    }
                    else
                    {
                        if (RoundsCompleted >= 2)   //CAN "SHOW" AFTER 2 ROUNDS.
                        {
                            int k = Random.Range(1, 5);  // 1, 2, 3, 4
                            switch (k)
                            {
                                //Show 1 out of 4. 
                                case 1:
                                    Bet(false);
                                    break;

                                case 2:
                                    Show();
                                    break;

                                case 3:
                                    Bet(false);
                                    break;

                                case 4:
                                    Bet(false);
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
            else //SomeOneShowed
            {
                Debug.Log((NumberOfPlayerShowed+1) + "PlayerShowed");

                if ((NumberOfPlayerShowed + 1) == PlayersList.Count)
                {
                    GameWon();
                }
                else
                {
                    Show();
                }
            }
        }


        private void GameWon()
        {
            WinningCardAnimation();
            Utils_X.DoActionAfterSecondsAsync(ShowGameWonScreen, 3);
        }

        private void ShowGameWonScreen()
        {
            PlayerPrefs.SetInt("diamond", (RedeemManager.GetCurrentDiamondCount() + TotalPot));
            GameWonPanel.SetActive(true);
            FinalWinnerText.text = "Yeah! Congo... " + TopRankers[0].GetComponent<Player>().name + " you WON !";
            string name = TopRankers[0].GetComponent<Player>().name;

            if(TopRankers[0].GetComponent<Player>().PlayerDefaultNumber==1)  //If Player is Winner.
            {
                if(TotalPot>MainMenu.LargestPotWin)
                {
                    MainMenu.LargestPotWin = TotalPot;
                    PlayerPrefs.SetInt("LargestPot", MainMenu.LargestPotWin);
                }

                if (MainMenu.UserCurrentChips> MainMenu.HighestChipsEver)
                {
                    MainMenu.HighestChipsEver = MainMenu.UserCurrentChips;
                    PlayerPrefs.SetInt("HighestChips", MainMenu.HighestChipsEver);
                }

                MainMenu.BestHandString = "A-A-A";
                PlayerPrefs.SetString("BestHand", MainMenu.BestHandString);
            }

            if (name == FB_Handler.instance.SavedUsername)
            {
                PlayerPrefs.SetInt("diamond", (RedeemManager.GetCurrentDiamondCount() + TotalPot));
                WebRequestManager.HttpGetAddCoin(FB_Handler.instance.SavedEmail, TotalPot.ToString(), () =>
                {
                    Debug.Log("Added win coin");
                    TopRankers[0].GetComponent<Player>().coin += TotalPot;
                });
            }
            
            StartCoroutine(StartTimer());
        }

        IEnumerator StartTimer()
        {
           
            int num = 5;
            while (num > 0)
            {

                NextGameText.text = "Next Game in " + num;
                num--;
                yield return new WaitForSeconds(1);

                GameInstance.new_instance.GetPlayersFromServer = false;
                SceneManager.LoadScene("Player");
               
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
                        else
                        {
                            PlayersList[i].GetComponent<Player>().SelectionUI.gameObject.SetActive(false);
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

                        if (CanBet() == false)    //
                        {
                            Interactable = false;
                        }
                        else
                        {
                            Interactable = true;
                        }


                    }
            }

            for (int i = 0; i < ObjectsToDisable.Count; i++)
            {
                    if (ObjectsToDisable[i].GetComponent<Button>() == true)
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

             BettingSlider.maxValue = MaxBetValueForUser;
             BettingSlider.minValue = MinBetValueForUser;

            
             if (BettingSlider.value >= ((MaxBetValueForUser + MinBetValueForUser) / 2))
             {
                 BetValue = MaxBetValueForUser;
             }
             else
             {
                 BetValue = MinBetValueForUser;
             }
        

             if(BetValue==0)
             {
                BetValue = MinBetValueForUser;
             }

             BettingValueText.text = BetValue.ToString();


        }

        public void AddBetButton()
        {
            BettingSlider.value = BettingSlider.maxValue;
            BetValue = (int)BettingSlider.value;
            BettingValueText.text = BetValue.ToString();

        }
        public void SubstractBetButton()
        {
            BettingSlider.value = BettingSlider.minValue;
            BetValue = (int)BettingSlider.value;
            BettingValueText.text = BetValue.ToString();
        }


        public void RefreshPotText()
        {
            TotalPotText.text = "Total Pot : " + TotalPot.ToString();
        }

        public void RefreshChipsText()
        {
            MainPlayerChips.text = MainMenu.UserCurrentChips.ToString();
        }

        public void ChangeScene(string Scene)
        {
            SceneManager.LoadScene(Scene);
            GameInstance.new_instance.ServerPlayersCoins.Clear();
            GameInstance.new_instance.ServerPlayersImages.Clear();
            GameInstance.new_instance.ServerPlayersNames.Clear();

            FB_Handler.instance.ResetMainMenu();
        }


        public void RandomToSeen(int PlayerNumber)
        {
            int k = Random.Range(1, 5);  //2, 3, 4, 5
            switch(k)
            {
                case 2:  PlayersList[PlayerNumber-1].GetComponent<Player>().StatusSeen = true;
                         PlayersList[PlayerNumber - 1].GetComponent<Player>().StatusText.text = "SEEN";
                    break;
                default: //Do nothing
                    break;
            }
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
                            else {/*DO NOTHING*/
            break; }
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

        public void ClearServerList()
        {
            GameInstance.new_instance.ServerPlayersCoins.Clear();
            GameInstance.new_instance.ServerPlayersImages.Clear();
            GameInstance.new_instance.ServerPlayersNames.Clear();
        }
            



      /*  #region  Card_Algorithm_For_TopChart

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
            for (int i = 0; i < 3; i++)
            {
                if (TopRankers[0].GetComponent<Player>().cardList[i].Rank == 1)
                {
                    TopRankers[0].GetComponent<Player>().cardList[i].Rank = 14;
                }
            }



            void Swap()
            {
                GameObject temp = TopRankers[CurrentPosition + 1];
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
                            else
                            {//DO NOTHING
                                break;
                            }
                        }
                        else {//DO NOTHING  break; }
                    }

                }
                else
                {
                    break;
                }
            }
        }
            
        #endregion   */


    }
}
