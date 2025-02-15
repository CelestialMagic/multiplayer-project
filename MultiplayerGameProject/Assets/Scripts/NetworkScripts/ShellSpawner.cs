using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShellSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject shellToSpawn;

    [SerializeField]
    private float spawnShellTimer; 

    [SerializeField]
    private GameObject spawnedShell;

    bool canSpawnAgain = false;


 public void SpawnShell(){
    StartCoroutine(SpawnAnotherShell());
 }

 public bool HasSpawnedShell(){
    return (spawnedShell != null);
 }

public void SpawnFirstShell(){
    spawnedShell = PhotonNetwork.Instantiate(shellToSpawn.name, this.transform.position, shellToSpawn.transform.rotation);
    spawnedShell.GetComponent<Shell>().SetShellSpawner(this);
}

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator SpawnAnotherShell(){
        yield return new WaitForSeconds(spawnShellTimer);
        spawnedShell = PhotonNetwork.Instantiate(shellToSpawn.name, this.transform.position, shellToSpawn.transform.rotation);
        spawnedShell.GetComponent<Shell>().SetShellSpawner(this);
    }
}
