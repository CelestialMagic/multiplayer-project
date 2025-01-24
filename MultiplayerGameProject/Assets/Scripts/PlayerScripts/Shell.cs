using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shell : MonoBehaviour
{

    [SerializeField]
    private string name, info;

    public string GetName(){
        return name; 
    }

    public string GetInfo(){
        return info; 
    }

    [PunRPC]
    private void DestroyShell(){
        PhotonNetwork.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }

    

    

}
