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
    private int roundLength; 

    private int currentRoundTime; 

    [SerializeField]
    private GameObject waitingUI; 

    public bool gameHasStarted = false; 
    public bool timerStarted = false; 

    private Coroutine timerCoroutine;

[SerializeField]    private PhotonView view; 



    


public void RefreshTimerUI(){
    Debug.Log("RefreshedUI");
    string minutes = (currentRoundTime/60).ToString("00");
    string seconds = (currentRoundTime % 60).ToString("00");
    countdownTimer.text = $"{minutes}:{seconds}";
}

private void StartTimer(){
    
    Debug.Log("Timer Started!");
    currentRoundTime = roundLength;
    RefreshTimerUI();

    timerCoroutine = StartCoroutine(Timer());
}


[PunRPC]
public void RPC_Countdown(){
    //RefreshTimerUI();
    string minutes = (currentRoundTime/60).ToString("00");
    string seconds = (currentRoundTime % 60).ToString("00");
    countdownTimer.text = $"{minutes}:{seconds}";

}

private IEnumerator Timer(){
    yield return new WaitForSeconds(1f);
    currentRoundTime -= 1; 
    view.RPC("RPC_Countdown", RpcTarget.All);
   
    //RefreshTimerUI();
    if(currentRoundTime <= 0){
        timerCoroutine = null;
        //RefreshTimerUI();
    }else{
        timerCoroutine = StartCoroutine(Timer());
        //RefreshTimerUI();
    }
}

private IEnumerator DelayStartGame(){
    yield return new WaitForSeconds(5f);
}





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
        if(gameHasStarted == true && timerStarted != true){
            waitingUI.SetActive(false);
            StartTimer();
            timerStarted = true; 
        }else{
        }
        if(PhotonNetwork.PlayerList.Length >= 5 && gameHasStarted != true){
            Debug.Log("The Game Can Start!");
            StartCoroutine(DelayStartGame());
            gameHasStarted = true;
            PhotonNetwork.CurrentRoom.IsVisible = false; 
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
