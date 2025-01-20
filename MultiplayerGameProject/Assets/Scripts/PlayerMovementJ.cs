using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovementJ : MonoBehaviour
{
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
    private InputAction forwardMovement, sideMovement, meleeMovement, jumpMovement;//InputActions to be mapped to player actions

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
    private Color color;//The player color (to be used by Photon eventually)

    [SerializeField]
    private Rigidbody rb;//The player's rigidbody

    private bool isGrounded = false;//Checks if player can jump again 



    // Start is called before the first frame update
    void Start()
    {
        //Locks cursor for rotating with mouse (commented out for now)
        Cursor.lockState = CursorLockMode.Locked;

//This code will be useful later for when we send RPC calls for players
//joining online 
        MeshRenderer[] childrenRenderers = GetComponentsInChildren<MeshRenderer>();
        Debug.Log(childrenRenderers);
        foreach (MeshRenderer m in childrenRenderers)
        {
            if(m.material.name == "Body (Instance)")
                m.material.color = color;
        }
        
    }

//Enables Input Actions
     void OnEnable(){
        forwardMovement.Enable();
        sideMovement.Enable();
        meleeMovement.Enable();
        jumpMovement.Enable();
    }

//Disables Input Actions
     void OnDisable(){
        forwardMovement.Disable();
        sideMovement.Disable();
        meleeMovement.Disable();
        jumpMovement.Disable();
    }

void FixedUpdate(){
    MovePlayer();
    
}

private void Update(){

    sideInput = GetSideInput();
    forwardInput = GetForwardInput();
    SpeedControl();

    rb.drag = groundDrag; 

        SimpleRotation();

    if(jumpMovement.ReadValue<float>() != 0 && isGrounded == true){
        rb.AddForce(Vector3.up * jumpForce);
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


private void OnTriggerEnter(Collider collision){
    Debug.Log("In Trigger");

//Checks if collider is a shell 
    if(collision.gameObject.tag == "Shell"){

        if(currentShell.GetComponent<Shell>().GetName() != collision.gameObject.GetComponent<Shell>().GetName()){
            //Hides the shell currently equipped 

            currentShell.SetActive(false);
            foreach(Shell s in playerShells)
            {
                //Compares currentShell to list of possible shells 
                if(s.GetName() == collision.gameObject.GetComponent<Shell>().GetName()){
                    currentShell = s.gameObject;
                    currentShell.SetActive(true);
                    break;
                    //Ends loop when found 
                }
                
            }

            Destroy(collision.gameObject);

        }

    }

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
