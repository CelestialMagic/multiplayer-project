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
    private Transform orientation;

    Vector3 moveDirection; 

    private float sideInput, forwardInput; 

    [SerializeField]
    private InputAction forwardMovement, sideMovement, meleeMovement, jumpMovement;//InputActions to be mapped to player actions

    [SerializeField]
    private CinemachineVirtualCamera followCam;

     private float rotation;//Value of Rotation

     [SerializeField]

     private float viewSpeed, viewRange;//Speed and range of view 

     [SerializeField]
     private GameObject currentShell; 

     [SerializeField]
     private List<Shell> playerShells; 

    [SerializeField]
    private GameObject prefab, modelView;//The player's prefab (to be used later for Photon)

    [SerializeField]
    private Color color;//The player color 

    [SerializeField]
    private Rigidbody rb;



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

    if(jumpMovement.ReadValue<float>() != 0){
        rb.AddForce(Vector3.up * jumpForce);
    }

    

}

private void MovePlayer(){
    moveDirection = orientation.forward * GetForwardInput() + orientation.right * GetSideInput(); 
    rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
}

public float GetSideInput(){
    return sideMovement.ReadValue<float>();

}
public float GetForwardInput(){
    return forwardMovement.ReadValue<float>();

}

private void SpeedControl(){
    Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    if(flatVelocity.magnitude > moveSpeed){
        Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
        rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
    }
}


private void SimpleRotation(){
    //Rotation with Mouse Code
    rotation += -Input.GetAxis("Mouse Y") * viewSpeed;
    rotation = Mathf.Clamp(rotation, -viewRange, viewRange);
    followCam.transform.localRotation = Quaternion.Euler(rotation, 0, 0);
    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * viewSpeed, 0);
}

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
}
