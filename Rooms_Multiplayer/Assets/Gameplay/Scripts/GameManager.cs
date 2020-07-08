using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Class to handle game logic, mainly timer, role assignment, and role switches
/// </summary>
namespace Multiplayer
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        // TIMER FIELDS
        private bool timerStarted = false;
        //private double timerIncrementValue; //how much time has passed since last update
        private double gameEndTime;
        private double captainEndTime;
        [SerializeField] private double timer; //how much time to have for a match
        [SerializeField] public double captainTimer; //how much time until captain can switch in
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI captainTimerText;

        // ROLE/TEAM FIELDS
        //modify for players
        List<string> roles = new List<string>(new string[] { "Captain", "Pilot", "Navigator", "None" });
        List<string> team = new List<string>(new string[] { "Pirate", "Admiral", "Pirate", "Pirate" });

        //giving new pilot ownership
        [SerializeField] private PhotonView submarineView; 

        //SUBMARINE POSITION AND NEARBY WIN STATUS FIELDS
        [SerializeField] private Transform goal;
        [SerializeField] private Transform submarinePosition;
        [SerializeField] private GameObject winOrLosePanel;
        [SerializeField] private TextMeshProUGUI winOrLose; //final text result
        private bool piratesWon = false;

        //BUTTON FOR BECOMING
        [SerializeField] private Button buttonBecomePilot;
        [SerializeField] private Button buttonBecomeNav;

        //COMPASS
        [SerializeField] private GameObject compass;

        //BUTTON FOR START GAME
        [SerializeField] private Button buttonStartGame;
        [SerializeField] private Button buttonReady;

        //Text who is ready
        [SerializeField] private TextMeshProUGUI playersReadyText;

        //NUMBER OF PEOPLE IN ROOM
        private int numberOfPlayers;
        //Check if currently captain, quicker than retrieving custom property
        bool amICaptain = false;
        public bool GameOver { get; set; }

        /// <summary>
        /// On start, get and store number of players and prep game if host/masterclient  
        /// </summary>
        private void Start()
        {
            numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
            if (PhotonNetwork.IsMasterClient)
            {
                buttonStartGame.interactable = false;
                buttonStartGame.gameObject.SetActive(true);

                Hashtable ready = new Hashtable();
                ready["ReadyPlayers"] = 1;
                PhotonNetwork.CurrentRoom.SetCustomProperties(ready);
            }
            
        }

        /// <summary>
        /// Method to set correct parameters when game has started and calls AssignRoles()
        /// </summary>
        public void StartGame()
        {
            // Only calllable by MasterClient
            if (PhotonNetwork.IsMasterClient)
            {
                // Set start button to false, only true when everyone is ready
                buttonStartGame.interactable = false;
                buttonStartGame.gameObject.SetActive(false);

                Debug.Log("Assigning Roles...");
                AssignRoles();

                // Game is not over and time not started
                GameOver = false;
                timerStarted = true;

                //Set custom room properties, shared game timer and captain timer
                Hashtable timeHash = new Hashtable();
                gameEndTime = PhotonNetwork.Time + timer;
                captainEndTime = PhotonNetwork.Time + captainTimer;
                timeHash["GameEndTime"] = gameEndTime;
                timeHash["CaptainEndTime"] = captainEndTime;
                PhotonNetwork.CurrentRoom.SetCustomProperties(timeHash);
            }
        }

        /// <summary>
        /// Triggered on clicking ready button, incrementes ReadyPlayers room property
        /// </summary>
        public void OnClick_Ready()
        {
            // Can only be ready once, so set interactable false
            buttonReady.interactable = false;
            Hashtable ready = new Hashtable();
            ready["ReadyPlayers"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["ReadyPlayers"] + 1;
            PhotonNetwork.CurrentRoom.SetCustomProperties(ready);
        }

        /// <summary>
        /// Update method primarily for timer and win state
        /// </summary>
        void Update()
        {
            if (timerStarted && !GameOver)
            {
                //Updating game time text
                double gameTimeLeft = gameEndTime - PhotonNetwork.Time; 
                timerText.text = ((int)gameTimeLeft).ToString();
                if (gameTimeLeft <= 0)
                {
                    timerStarted = false;
                    timerText.text = "0";
                    GameOver = true;
                }
                //Updating captain time text
                double captainTimeLeft = captainEndTime - PhotonNetwork.Time;
                captainTimerText.text = ((int)captainTimeLeft).ToString();
                if (captainTimeLeft <= 0 )
                {
                    captainTimerText.text = "Captain Power!";
                }

                //Check if captain can exercise power
                if (amICaptain && captainTimeLeft <= 0)
                {
                    buttonBecomePilot.interactable = true;
                    buttonBecomeNav.interactable = numberOfPlayers >= 3;
                }     
            }

            //For game end states
            if (CheckAtGoal() && !GameOver)
            {
                photonView.RPC("RPC_PirateWin", RpcTarget.All);
                
            }
            else if (GameOver && !piratesWon)
            {
                photonView.RPC("RPC_AdmiralWin", RpcTarget.All);
            }
        }

        /// <summary>
        /// RPC for when pirates win
        /// </summary>
        [PunRPC]
        private void RPC_PirateWin()
        {
            print("PIRATES WIN");
            PirateWin();
            piratesWon = true;
            GameOver = true;
            FindObjectOfType<PlayerMovement>().canMove = false;
            enabled = false;
        }

        /// <summary>
        /// RPC for when admiral wins
        /// </summary>
        [PunRPC]
        private void RPC_AdmiralWin()
        {
            print("ADMIRAL WIN");
            AdmiralWin();
            GameOver = true;
            FindObjectOfType<PlayerMovement>().canMove = false;
            enabled = false;
        }

        /// <summary>
        /// For timer updates and ready player updates
        /// </summary>
        /// <param name="propertiesThatChanged"> Hashtable with updated room properties </param>
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            // Check if game has started
            if (propertiesThatChanged.ContainsKey("GameEndTime"))
            {
                gameEndTime = (double)propertiesThatChanged["GameEndTime"];
                GameOver = false;
                timerStarted = true;

                //Allow people to move sub
                FindObjectOfType<PlayerMovement>().canMove = true;

                // Remove game button
                buttonReady.gameObject.SetActive(false);
            }

            // Store end time of game
            if (propertiesThatChanged.ContainsKey("CaptainEndTime"))
            {
                captainEndTime = (double)propertiesThatChanged["CaptainEndTime"];
            }

            // Handling changes in ReadyPlayer Property
            if (propertiesThatChanged.ContainsKey("ReadyPlayers"))
            {
                int readyPlayers = (int)propertiesThatChanged["ReadyPlayers"];
                // Update text num players ready
                if (PhotonNetwork.IsMasterClient)
                {
                    playersReadyText.text = readyPlayers.ToString() + "/" + numberOfPlayers.ToString() + " players ready";
                }
                // Allow people to ready up once MasterClient is connected
                if (readyPlayers == 1 && !PhotonNetwork.IsMasterClient)
                {
                    buttonReady.interactable = true;
                    buttonReady.gameObject.SetActive(true);
                }
                // Allow MasterClient to start game once everyone has ready up
                if (readyPlayers == PhotonNetwork.CurrentRoom.PlayerCount && PhotonNetwork.IsMasterClient)
                {
                    buttonStartGame.interactable = true;
                }
            }
        }
        
        /// <summary>
        /// Assigning roles at the beginning of the game
        /// </summary>
        public void AssignRoles()
        {
            //Get roles for number of players
            var currentRoles = roles.GetRange(0, numberOfPlayers);
            var currentTeam = team.GetRange(0, numberOfPlayers);

            //Randomize roles
            var shuffledRoles = currentRoles.OrderBy(a => Guid.NewGuid()).ToList();
            var shuffledTeam = currentTeam.OrderBy(a => Guid.NewGuid()).ToList();

            int i = 0;
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                //Assign role and team to player
                Hashtable playerHashtable = new Hashtable();
                playerHashtable["Role"] = shuffledRoles[i];
                playerHashtable["Team"] = shuffledTeam[i];
                p.SetCustomProperties(playerHashtable);

                //If player is pilot, get ownership
                if (shuffledRoles[i].Equals("Pilot"))
                {
                    submarineView.TransferOwnership(p);
                }
                i++;
            }
        }

        /// <summary>
        /// Check if submarine is at treasure
        /// </summary>
        /// <returns> bool : true if within 5f, false if beyond 5f </returns>
        private bool CheckAtGoal()
        {
            return Vector3.Distance(submarinePosition.position, goal.position) <= 5f;
        }

        /// <summary>
        /// Displays correct Pirate Win text depending on your team
        /// </summary>
        private void PirateWin()
        {
            if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team")) { return; }
            winOrLosePanel.SetActive(true);
            winOrLose.text = "Pirates Win!";
        }

        /// <summary>
        /// Displays correct Admiral Win text depending on your team
        /// </summary>
        private void AdmiralWin()
        {
            if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team")) { return; }
            winOrLosePanel.SetActive(true);
            winOrLose.text = "Admiral Wins!";
        }

        #region Role Changes
        /// <summary>
        /// When button Become Pilot is triggered, handles submarine ownership and role changes
        /// Updates CaptainEndTime
        /// </summary>
        public void OnClick_BecomePilot()
        {
            //Get ownership
            submarineView.RequestOwnership();
            //Role updates for everyone
            photonView.RPC("RPC_NewPilot", RpcTarget.All);
            //Update captain timer
            Hashtable newRoom = new Hashtable();
            newRoom["CaptainEndTime"] = PhotonNetwork.Time + captainTimer;
            PhotonNetwork.CurrentRoom.SetCustomProperties(newRoom);
        }

        /// <summary>
        /// RPC for when Captain becomes pilot, handling role changes
        /// </summary>
        [PunRPC]
        private void RPC_NewPilot()
        {
            if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role")) 
            {
                Debug.LogError("NO ROLES??? WHEN PILOT SWITCH");
                return; 
            }
            Hashtable hash = new Hashtable();
            string currentRole = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (currentRole.Equals("Captain"))
            {
                hash["Role"] = "Pilot";
            } else if (currentRole.Equals("Pilot"))
            {
                if (numberOfPlayers == 2)
                {
                    hash["Role"] = "Captain";
                } else
                {
                    hash["Role"] = roles[3 % numberOfPlayers];
                }
            } else if (currentRole.Equals("None"))
            {
                hash["Role"] = roles[4 % numberOfPlayers];
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        /// <summary>
        /// When button Become Nav is triggered handles role changes
        /// Updates CaptainEndTime
        /// </summary>
        public void OnClick_BecomeNavigator()
        {
            //Update roles for all
            photonView.RPC("RPC_NewNavigator", RpcTarget.All);
            //Update captain timer
            Hashtable newRoom = new Hashtable();
            newRoom["CaptainEndTime"] = PhotonNetwork.Time + captainTimer;
            PhotonNetwork.CurrentRoom.SetCustomProperties(newRoom);
        }

        /// <summary>
        /// RPC for when Captain becomes navigator, handling role changes
        /// </summary>
        [PunRPC]
        private void RPC_NewNavigator()
        {
            if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
            {
                Debug.LogError("NO ROLES??? WHEN NAVIGATOR SWITCH");
                return;
            }
            Hashtable hash = new Hashtable();
            string currentRole = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (currentRole.Equals("Captain"))
            {
                hash["Role"] = "Navigator";
            }
            else if (currentRole.Equals("Navigator"))
            {
                hash["Role"] = roles[3 % numberOfPlayers];
            }
            else if (currentRole.Equals("None"))
            {
                hash["Role"] = roles[4 % numberOfPlayers];
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        /// <summary>
        /// Keeps track of changes to player properties
        /// </summary>
        /// <param name="targetPlayer">The player who's properties got changed</param>
        /// <param name="changedProps">The changed properties</param>
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            // Check that the changed player is not null and is the local player
            if (targetPlayer != null && targetPlayer.IsLocal)
            {
                if (changedProps.ContainsKey("Role"))
                {
                    string myNewRole = (string)changedProps["Role"];

                    //If new role is navigator, then set compass active
                    compass.SetActive(myNewRole.Equals("Navigator"));

                    //Set correct captain boolean
                    amICaptain = myNewRole.Equals("Captain");
                    buttonBecomePilot.gameObject.SetActive(amICaptain);
                    buttonBecomeNav.gameObject.SetActive(amICaptain);
                    buttonBecomePilot.interactable = false;
                    buttonBecomeNav.interactable = false;

                    Debug.Log("I AM THE " + myNewRole + " NOW!");
                }
            }
        }

        #endregion

        /// <summary>
        /// Load first scene when you leave the room
        /// </summary>
        public override void OnLeftRoom()
        {
            print("Left Room");
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Method to leave the room
        /// </summary>
        public void LeaveRoom()
        {
            Time.timeScale = 0;
            this.enabled = false;
            //print("Destroy all");
            //foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>())
            //{
            //    Destroy(o);
            //}

            print("Leaving Room");
            
            PhotonNetwork.LeaveRoom();
        }
    }
}

