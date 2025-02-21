using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;

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
    private TMP_Text countdownTimer, leaderboardText, roomJoinTimerText;//Text representing a countdown

    [SerializeField]
    private int roundLength, roomJoinTime; 

    private int currentRoundTime; 

    [SerializeField]
    private GameObject waitingUI, endUI; 

    public bool gameHasStarted = false; 

    public bool gameHasEnded = false;
    public bool timerStarted = false; 

    public bool joinCountdownStarted = false; 

    private Coroutine timerCoroutine, roomJoinCoroutine;


[SerializeField] private PhotonView view; 


[SerializeField] private int startGamePlayers;

[SerializeField]
private List<int> scores;

public List<int> GetScores(){
    return scores; 
}

    


public void RefreshTimerUI(){
    Debug.Log("RefreshedUI");
    string minutes = (currentRoundTime/60).ToString("00");
    string seconds = (currentRoundTime % 60).ToString("00");
    countdownTimer.text = $"Timer: {minutes}:{seconds}";
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
    countdownTimer.text = $"Timer: {minutes}:{seconds}";

}

[PunRPC]
public void RPC_JoinRoomCountdown(){
    //RefreshTimerUI();
    string minutes = (0).ToString("00");
    string seconds = (roomJoinTime).ToString("00");
    roomJoinTimerText.text = $"Match Starting In: {minutes}:{seconds}";

}

private IEnumerator Timer(){
    yield return new WaitForSeconds(1f);
    currentRoundTime -= 1; 
    view.RPC("RPC_Countdown", RpcTarget.All);
   
    //RefreshTimerUI();
    if(currentRoundTime <= 0){
        timerCoroutine = null;
        gameHasEnded = true;
        endUI.SetActive(true);
        DetermineFinalScores();
        //RefreshTimerUI();
    }else{
        timerCoroutine = StartCoroutine(Timer());
        
        //RefreshTimerUI();
    }
}


private IEnumerator DelayStartGame(){
    yield return new WaitForSeconds(1f);
    roomJoinTime -= 1; 
    view.RPC("RPC_JoinRoomCountdown", RpcTarget.All);

    if(roomJoinTime <= 0){
        roomJoinCoroutine = null;
        gameHasStarted = true;
    }else{
        roomJoinCoroutine = StartCoroutine(DelayStartGame());
        
    }
}





    // Start is called before the first frame update
    void Start()
    {
        if(gameHasStarted == false)
            waitingUI.SetActive(true);
        if (PhotonNetwork.IsConnected){
            OnConnectedToMaster();

        }else
            PhotonNetwork.ConnectUsingSettings();
        
    }

    void Update(){
        //Used to start the game
        if(gameHasStarted == true && timerStarted != true){
            waitingUI.SetActive(false);
            StartTimer();
            timerStarted = true; 
        }else{
        if(PhotonNetwork.PlayerList.Length >= startGamePlayers && gameHasStarted != true && joinCountdownStarted == false){
            PhotonNetwork.CurrentRoom.IsVisible = false; 
            joinCountdownStarted = true; 
            StartCoroutine(DelayStartGame());
            
        }
        }
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinRandomRoom();
        //PhotonNetwork.JoinRandomOrCreateRoom();

    }
    public override void OnJoinedRoom(){
        PhotonNetwork.Instantiate(playerPrefab.name, GetRandomLocation(), Quaternion.identity);
        view.RPC("RPC_JoinRoomCountdown", RpcTarget.All);
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

    private void DetermineFinalScores(){
        List <PlayerMovementJ> players = FindObjectsOfType<PlayerMovementJ>().ToList();
        string leaderboard = "";
        int place = 1; 

        foreach(PlayerMovementJ player in players){
            scores.Add(player.GetScore());
        }
        scores.Sort();
        scores.Reverse(); 

        foreach(int s in scores){
            switch(place){
                case 1:
                leaderboard += "1st";
                break;

                case 2:
                leaderboard += "2nd";
                break;

                case 3:
                leaderboard += "3rd";
                break;


                default:
                leaderboard += $"{place}th";
                break;
            }
            leaderboard += $" {s} \n";
            place++;

            
        }
        leaderboardText.text = leaderboard;

        


    }


    
}
