using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shell : MonoBehaviour
{

    [SerializeField]
    private string name;

    public string GetName(){
        return name; 
    }

    [PunRPC]
    private void DestroyShell(){
        PhotonNetwork.Destroy(this.gameObject);
    }

    

    

}
