using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SimpleLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Vector3 location; 



    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinRandomOrCreateRoom();

    }
    public override void OnJoinedRoom(){
        PhotonNetwork.Instantiate(playerPrefab.name, location ,Quaternion.identity);
    }

    
}
