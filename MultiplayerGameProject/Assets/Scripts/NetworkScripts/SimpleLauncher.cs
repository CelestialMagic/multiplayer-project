using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SimpleLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab, gameUI;

    [SerializeField]
    private Vector3 location; 

    [SerializeField]
    private List<Vector3> spawnLocations; 

    [SerializeField]
    private List<ShellSpawner> shellSpawners; 



    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected){
            OnConnectedToMaster();

        }else
            PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinRandomOrCreateRoom();

    }
    public override void OnJoinedRoom(){
        PhotonNetwork.Instantiate(playerPrefab.name, GetRandomLocation(), Quaternion.identity);
        if(PhotonNetwork.IsMasterClient){
            foreach(ShellSpawner s in shellSpawners){
            if(s.HasSpawnedShell() == false)
                s.SpawnFirstShell();
        }

        }
        
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

//Returns a random spot to respawn
    public Vector3 GetRandomLocation(){
        return spawnLocations[Random.Range(0, spawnLocations.Count)];
    }

    
}
