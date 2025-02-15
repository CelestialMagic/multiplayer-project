using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shell : MonoBehaviour
{

    [SerializeField]
    private string name, info;//Basic Info to Display About Shell

    [SerializeField]
    private Sprite icon; 

[Header("Shell-Specific Attributes")]
    [SerializeField]
    private float speedModifier, jumpModifier, healthModifier, attackRadiusModifier; 

    [SerializeField]
    private PhotonView view;

    public string GetName(){
        return name; 
    }

    public string GetInfo(){
        return info; 
    }

    public Sprite GetIcon(){
        return icon; 
    }

     public float GetSpeedModifier(){
        return speedModifier; 
    }

    public float GetJumpModifier(){
        return jumpModifier; 
    }

    public float GetHealthModifier(){
        return healthModifier; 
    }

    public float GetAttackRadiusModifier(){
        return attackRadiusModifier; 
    }









    [PunRPC]
    private void DestroyShell(){
        PhotonNetwork.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void ShowAndHideShell(){
        view.RPC("RPC_HideShell", RpcTarget.All);
        view.RPC("RPC_ShowShell", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_HideShell(){
        gameObject.SetActive(false);
        
    }

    [PunRPC]
    public void RPC_ShowShell(){
        gameObject.SetActive(true);
    }


    

    

}
