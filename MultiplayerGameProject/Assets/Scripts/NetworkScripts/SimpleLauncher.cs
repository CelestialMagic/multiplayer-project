using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SimpleLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int maxPlayers;

    [SerializeField]
    private GameObject playerPrefab, gameUI;

    [SerializeField]
    private Vector3 location; 

    [SerializeField]
    private List<Vector3> spawnLocations; 

    [SerializeField]
    private TMP_Text countdownTimer;//Text representing a countdown

    [SerializeField]
    private GameObject waitingUI; 

    public bool gameHasStarted = false; 



    // Start is called before the first frame update
    void Start()
    {
        waitingUI.SetActive(true);
        if (PhotonNetwork.IsConnected){
            OnConnectedToMaster();

        }else
            PhotonNetwork.ConnectUsingSettings();
        
    }

    void Update(){
        if(gameHasStarted == true){
            waitingUI.SetActive(false);
        }else{
            
        }

        if(PhotonNetwork.PlayerList.Length >= 3){
            Debug.Log("The Game Can Start!");
            gameHasStarted = true;
        }
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinRandomRoom();
        //PhotonNetwork.JoinRandomOrCreateRoom();

    }
    public override void OnJoinedRoom(){
        PhotonNetwork.Instantiate(playerPrefab.name, GetRandomLocation(), Quaternion.identity);
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    //Code used when leaving room
    public void LeaveRoom()
    {
        Debug.Log(PhotonNetwork.LeaveRoom());
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

//Returns a random spot to respawn
    public Vector3 GetRandomLocation(){
        return spawnLocations[Random.Range(0, spawnLocations.Count)];
    }

    
}
