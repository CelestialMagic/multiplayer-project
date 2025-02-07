using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shell : MonoBehaviour
{

    [SerializeField]
    private string name, info;//Basic Info to Display About Shell

[Header("Shell-Specific Attributes")]
    [SerializeField]
    private float speedModifier, jumpModifier, healthModifier, attackRadiusModifier; 

    public string GetName(){
        return name; 
    }

    public string GetInfo(){
        return info; 
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

    

    

}
