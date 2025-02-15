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

       [SerializeField]
    private ShellSpawner shellSpawner; 

[Header("Shell-Specific Attributes")]
    [SerializeField]
    private float speedModifier, jumpModifier, healthModifier, attackRadiusModifier; 

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





 

    public void SetShellSpawner(ShellSpawner s){
        shellSpawner = s; 
    }




    [PunRPC]
    private void DestroyShell(){
        shellSpawner.SpawnShell(); 
        PhotonNetwork.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }

    

    

}
