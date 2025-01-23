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



    // Start is called before the first frame update
    void Start()
    {
        
        //Locks cursor for rotating with mouse (commented out for now)
        //Cursor.lockState = CursorLockMode.Locked;

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
private void RPC_EquipShell(bool activation){
    Debug.Log("Sending Shell RPC");
    currentShell.SetActive(activation);
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
}
    
}

private void Update(){
if (view.IsMine){
    sideInput = GetSideInput();
    forwardInput = GetForwardInput();
    SpeedControl();

    rb.drag = groundDrag; 

        SimpleRotation();

    if(jumpMovement.ReadValue<float>() != 0 && isGrounded == true){
        rb.AddForce(Vector3.up * jumpForce);
    }
}
    

}

private void MovePlayer(){
    moveDirection = orientation.forward * GetForwardInput() + orientation.right * GetSideInput(); 
    rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
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
        shellUI.SetActive(true);
    }


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
                view.RPC("RPC_EquipShell", RpcTarget.All, false);
            
            //currentShell.SetActive(false);
            foreach(Shell s in playerShells)
            {
                //Compares currentShell to list of possible shells 
                if(s.GetName() == collision.gameObject.GetComponent<Shell>().GetName()){
                    currentShell = s.gameObject;
                    
                    //view.RPC("RPC_EquipShell", RpcTarget.All, true);
                    
                    //currentShell.SetActive(true);
                    break;
                    //Ends loop when found 
                }
                
            }

            PhotonView photonView = collision.gameObject.GetComponent<PhotonView>();
            if(photonView)
                photonView.RPC("DestroyShell", RpcTarget.All);
            shellUI.SetActive(false);


        }
        }
        }

    }

}

private void OnTriggerExit(Collider collision){
    shellUI.SetActive(false);

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



}
