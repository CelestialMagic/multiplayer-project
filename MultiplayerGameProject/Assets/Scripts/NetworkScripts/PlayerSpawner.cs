using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField] GameObject playerPrefab;//The player's prefab

    [SerializeField] Vector3 spawnLocation;//The location to spawn from


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnLocation, playerPrefab.transform.rotation);
        
    }

    
}
