using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class GameLauncher : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private byte maxPlayersPerRoom;//The maximum number of players allowed per instance of the game

    [SerializeField]
    private GameObject lobbyDisplay;//A game object for the display 

    [SerializeField]
    private TMP_InputField createRoomInput, joinRoomInput;//Input fields for creating and joining rooms

    [SerializeField]
    private string levelName;//The name of the scene to join

    [SerializeField]
    private string gameVersion;//The version of the game  


    void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        lobbyDisplay.SetActive(false);
    }

//The Connect() method is responsible for the connection process
//The if statement represents the following:
//- If we are already connected, we join a random room
//- If not, then we connect to the Photon Network 
    public void Connect(){

        if (PhotonNetwork.IsConnected){
            PhotonNetwork.JoinLobby();
        }else{
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster(){
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        lobbyDisplay.SetActive(true);
        PhotonNetwork.JoinLobby();

    }

//OnDisconnected disables the lobby display 
    public override void OnDisconnected(DisconnectCause cause){
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        lobbyDisplay.SetActive(false);
    }

//OnJoinLobby() activates the join and create menu "lobbyDisplay"
    public override void OnJoinedLobby(){
        Debug.Log("Successfully Joined Lobby!");
        lobbyDisplay.SetActive(true);
    }


    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No random available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
    }

//CreateRoom() creates a room based on user input
    public void CreateRoom(){
        PhotonNetwork.CreateRoom(createRoomInput.text);
    }
//JoinRoom() joins a room based on the room name inputted 
    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

//OnJoinedRoom() loads the room and syncs the scene 
    public override void OnJoinedRoom(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(levelName);
        }
    }

}
