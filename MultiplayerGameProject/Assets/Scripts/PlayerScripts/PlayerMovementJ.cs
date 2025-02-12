using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Photon.Pun;
using TMPro;

public class PlayerMovementJ : MonoBehaviour{

    /*
Player Fields 
*/
    [SerializeField]
    private float moveSpeed, rotationSpeed, groundDrag, jumpForce;//floats representing speed to move by

    [SerializeField]
    private Transform orientation;//Orientation of player 

    Vector3 moveDirection;//Direction to move by

    private float sideInput, forwardInput;//Not currently used 

    [SerializeField]
    private InputAction forwardMovement, sideMovement, meleeMovement, jumpMovement, selectMovement;//InputActions to be mapped to player actions

    [SerializeField]
    private CinemachineVirtualCamera followCam;//The camera assigned to the player

     private float rotation;//Value of Rotation

     [SerializeField]

     private float viewSpeed, viewRange;//Speed and range of view 

     [SerializeField]
     private GameObject currentShell;//The currently equipped shell 

     [SerializeField]
     private List<Shell> playerShells;//The list of available shells to swap out

    [SerializeField]
    private GameObject prefab, modelView;//The player's prefab (to be used later for Photon)

    [SerializeField]
    private Rigidbody rb;//The player's rigidbody

    private bool isGrounded = false;//Checks if player can jump again 

    [SerializeField]
    private GameObject shellUI; //A popup for when player is in range of shells 

    [SerializeField]
    private TMP_Text shellText;//Display text for collided shells 

    [SerializeField]
    private PhotonView view;//The PhotonView Component

    [SerializeField]
    private List<Color> colorPalettes;//A list of colors to randomly select when online 

    [SerializeField]
    private static List<GameObject> onlinePlayers = new List<GameObject>();//List of players currently online in lobby

    private int currentShellIndex; 

    private bool hasShell = true; 

    private bool vulnerable = false; 

[SerializeField]
    private GameObject damageVolume;//A volume for damaging 

    [SerializeField]
    private float invulnerabilityTime;




    public float attackRange = 2f; // Range of the melee attack
    public float attackAngle = 60f; // Angle of the attack cone

    public float attackCooldown = 0.5f; // Cooldown time between attacks


    private float lastAttackTime = 0f; // Tracks the time of the last attack

    private bool canAttack = true;

[SerializeField]
    private PlayerInfoDisplay playerUI;

[SerializeField]
private Shell blankShell; 

private int timesHit = 0; 



    // Start is called before the first frame update
    void Start()
    {
        
        //Locks cursor for rotating with mouse (commented out for now)
        //Cursor.lockState = CursorLockMode.Locked;
        playerUI = GameObject.FindObjectOfType<PlayerInfoDisplay>();

    if(!view.IsMine){
    followCam.enabled = false;
    }
        Color color = colorPalettes[(int)Random.Range(0, colorPalettes.Count - 1)];//Picks a random color 
        //RPC call
        if (view)
            this.view.RPC("RPC_SendColor", RpcTarget.All, new Vector3(color.r, color.g, color.b));
        
    }

//Returns the active player objects 
 public static List<GameObject> GetOnlinePlayers()
    {
        return onlinePlayers;
    }


 //RPC_SendColor() sets the player to a random color and updates the other
//players' screens. 
[PunRPC]
private void RPC_SendColor(Vector3 color){
    PhotonNetwork.AutomaticallySyncScene = true;
    MeshRenderer[] childrenRenderers = GetComponentsInChildren<MeshRenderer>();
        Debug.Log(childrenRenderers);
        foreach (MeshRenderer m in childrenRenderers)
        {
            if(m.material.name == "Body (Instance)")
                m.material.color = new Color(color.x, color.y, color.z);
        }
}


[PunRPC]
private void RPC_RemoveShell(int value){
    Debug.Log("Sending Shell RPC");
    //
    playerShells[value].gameObject.SetActive(false);
    currentShell.SetActive(false);
    currentShell = blankShell.gameObject; 
}

[PunRPC]
private void RPC_DeactivateAllShells(){
    foreach(Shell s in playerShells){
        s.gameObject.SetActive(false);
    }
    currentShell = blankShell.gameObject; 

}

[PunRPC]
private void RPC_Respawn(){
    Debug.Log("Respawning");
    this.gameObject.SetActive(false);
    currentShell = playerShells[0].gameObject;
    this.gameObject.transform.position = new Vector3(0, 10, 0);
}


[PunRPC]
private void RPC_VisibleAgain(){
    this.gameObject.SetActive(true);
    playerShells[0].gameObject.SetActive(true);
    
}



[PunRPC]
private void RPC_EquipShell(int value){
    Debug.Log("Sending Shell RPC");
    vulnerable = false; 
    playerShells[value].gameObject.SetActive(true);
}

[PunRPC]
private void RPC_DamageVolumeEnable(bool value){
    
    damageVolume.SetActive(value);
    
}

[PunRPC]
private void RPC_ShowDamageScale(float value){
    
    damageVolume.transform.localScale = new Vector3(2 * value, 2 * value, 2 * value);
    
}



//Resets the list of active players
private void OnDestroy(){
    {
        onlinePlayers = new List<GameObject>();
    }

}





    private void Awake(){
        onlinePlayers.Add(this.gameObject);
    }


//Returns the PhotonView Component
    public PhotonView GetView(){
        return view;
    }



//Enables Input Actions
     void OnEnable(){
        forwardMovement.Enable();
        sideMovement.Enable();
        meleeMovement.Enable();
        jumpMovement.Enable();
        selectMovement.Enable();
    }

//Disables Input Actions
     void OnDisable(){
        forwardMovement.Disable();
        sideMovement.Disable();
        meleeMovement.Disable();
        jumpMovement.Disable();
        selectMovement.Disable();
    }

void FixedUpdate(){

if (view.IsMine){
    MovePlayer();
    
    //Jumping with Modifier
    if(jumpMovement.ReadValue<float>() != 0 && isGrounded == true){
        rb.AddForce(Vector3.up * (jumpForce * currentShell.GetComponent<Shell>().GetJumpModifier()));
    }

}
    
}

private void Update(){
if (view.IsMine){
    if(meleeMovement.ReadValue<float>() != 0 && canAttack == true){
        StartCoroutine(AttackPeriod());
        

    }
        
    sideInput = GetSideInput();
    forwardInput = GetForwardInput();
    SpeedControl();

    SimpleRotation();

    

    rb.drag = groundDrag; 
    playerUI.SetCurrentShellDisplay(currentShell.GetComponent<Shell>().GetName(), currentShell.GetComponent<Shell>().GetInfo(), currentShell.GetComponent<Shell>().GetIcon());

    //sphereCollider.radius *= currentShell.GetComponent<Shell>().GetAttackRadiusModifier();
    view.RPC("RPC_ShowDamageScale", RpcTarget.All, currentShell.GetComponent<Shell>().GetAttackRadiusModifier());
    
    
}
    

}

private void MovePlayer(){
    moveDirection = orientation.forward * GetForwardInput() + orientation.right * GetSideInput(); 
    rb.AddForce(moveDirection.normalized * (moveSpeed * currentShell.GetComponent<Shell>().GetSpeedModifier()), ForceMode.Force);
}

//Gets the side input of the player 
public float GetSideInput(){
    return sideMovement.ReadValue<float>();

}

//Gets the forward movement of the player
public float GetForwardInput(){
    return forwardMovement.ReadValue<float>();

}

//Controls the drag of the player moving 
private void SpeedControl(){
    Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    if(flatVelocity.magnitude > moveSpeed){
        Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
        rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
    }
}

//Current player rotation scheme 
private void SimpleRotation(){
    //Rotation with Mouse Code
    rotation += -Input.GetAxis("Mouse Y") * viewSpeed;
    rotation = Mathf.Clamp(rotation, -viewRange, viewRange);
    followCam.transform.localRotation = Quaternion.Euler(rotation, 0, 0);
    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * viewSpeed, 0);
}

//A potential mode to rotate around the model
private void ModelRotation(){
    //Rotation with Mouse Code
    rotation += -Input.GetAxis("Mouse Y") * viewSpeed;
    rotation = Mathf.Clamp(rotation, -viewRange, viewRange);
    followCam.transform.localRotation = Quaternion.Euler(rotation, 0, 0);
    modelView.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * viewSpeed, 0);
}

//Displays the Popup for the shell and ensures it does not appear when shell is the same as currently equipped 
private void OnTriggerEnter(Collider collision){
    if(collision.gameObject.tag == "Shell" && currentShell.GetComponent<Shell>().GetName() != collision.gameObject.GetComponent<Shell>().GetName()){
        shellText.text = $"Press E to collect the {collision.gameObject.GetComponent<Shell>().GetName()}!";
        //shellUI.SetActive(true);
    }else if (collision.gameObject.tag == "Claws" && collision.gameObject != this.damageVolume)
        Debug.Log("Uh oh! Claws!");


}


//Contains logic for remaining in range of the shell to collect 
private void OnTriggerStay(Collider collision) {
    //Debug.Log("In Trigger");

//Checks if collider is a shell 
    if(collision.gameObject.tag == "Shell"){

        

         if(currentShell.GetComponent<Shell>().GetName() != collision.gameObject.GetComponent<Shell>().GetName()){
            shellText.text = $"Press E to collect the {collision.gameObject.GetComponent<Shell>().GetName()}!";

         
         if(view.IsMine){
        if(selectMovement.ReadValue<float>() != 0){
            //Hides the shell currently equipped 

            //if(view)
                view.RPC("RPC_RemoveShell", RpcTarget.All, currentShellIndex);
            
            //currentShell.SetActive(false);
            for (int i = 0; i < playerShells.Count; i++)
            {
                //Compares currentShell to list of possible shells 
                if(playerShells[i].GetName() == collision.gameObject.GetComponent<Shell>().GetName()){
                    currentShell = playerShells[i].gameObject;
                    currentShellIndex = i; 
                    view.RPC("RPC_EquipShell", RpcTarget.All, i);
                    
                    //currentShell.SetActive(true);
                    break;
                    //Ends loop when found 
                }
                
            }

            PhotonView photonView = collision.gameObject.GetComponent<PhotonView>();
            if(photonView)
                photonView.RPC("DestroyShell", RpcTarget.All);
            //shellUI.SetActive(false);


        }
        }
        }

    }else if (collision.gameObject.tag == "Claws"){
        if(view){
            if(collision.gameObject != this.damageVolume){
                if(vulnerable){
                StartCoroutine(Respawning());
                }else{
                if(timesHit >= currentShell.GetComponent<Shell>().GetHealthModifier()){
                    view.RPC("RPC_RemoveShell", RpcTarget.All, currentShellIndex);
                    
                }else{
                    StartCoroutine(Invulnerability());
                }
                


            }
                
             }
        
    }

}
}

private void OnTriggerExit(Collider collision){
    //shellUI.SetActive(false);

}

//Used to check if player is on the ground
private void OnCollisionStay(Collision collision){
    if(collision.gameObject.tag == "Ground"){
        
        isGrounded = true;
        Debug.Log(isGrounded);
    }
}

//Used to check if player leaves the ground 
private void OnCollisionExit(Collision collision){
    isGrounded = false; 
    Debug.Log(isGrounded);
}

//Potential RPC Call







 IEnumerator AttackPeriod()
    {
        canAttack = false; 
        view.RPC("RPC_DamageVolumeEnable", RpcTarget.All, true);
        yield return new WaitForSeconds(attackCooldown);
        view.RPC("RPC_DamageVolumeEnable", RpcTarget.All, false);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; 
    }
IEnumerator Respawning(){
    //Resets hit ability to destroy shell
    timesHit = 0; 
    //StopCoroutine(AttackPeriod());

    view.RPC("RPC_DeactivateAllShells", RpcTarget.All);
    view.RPC("RPC_Respawn", RpcTarget.All);
    view.RPC("RPC_DamageVolumeEnable", RpcTarget.All, false);
    view.RPC("RPC_VisibleAgain", RpcTarget.All);
    vulnerable = false;
    canAttack = true; 
    
    yield return new WaitForSeconds(0.1f);
    
    

}

IEnumerator Invulnerability(){

    timesHit++; 
    yield return new WaitForSeconds(invulnerabilityTime);
    vulnerable = true; 
    
    

}



}

