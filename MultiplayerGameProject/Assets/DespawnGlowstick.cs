using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DespawnAfterTime : MonoBehaviour
{
    public float despawnTime = 10f; // Time in seconds before the glowstick disappears

    public PhotonView photonView;

    void Start()
    {
        if(photonView){
            photonView.RPC("RPC_DestroySelf", RpcTarget.All);

        }
        // Destroy the GameObject after the specified time
        
    }

    [PunRPC]
    private void RPC_DestroySelf(){
        Destroy(gameObject, despawnTime);

    }

}