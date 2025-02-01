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
        PhotonNetwork.Instantiate(playerPrefab.name, location ,Quaternion.identity);
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

    
}
