using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

//Code by Jessie Archer
public class Player : MonoBehaviour
{

/*
Player Fields 
*/
    [SerializeField]
    private float moveSpeed, rotationSpeed, groundDrag;//floats representing speed to move by

    [SerializeField]
    private Transform orientation;

    Vector3 moveDirection; 

    private float sideInput, forwardInput; 

    [SerializeField]
    private InputAction forwardMovement, sideMovement, meleeMovement;//InputActions to be mapped to player actions

    [SerializeField]
    private CinemachineVirtualCamera followCam;

     private float rotation;//Value of Rotation

     [SerializeField]

     private float viewSpeed, viewRange;//Speed and range of view 






    [SerializeField]
    private GameObject prefab;//The player's prefab (to be used later for Photon)

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
    }

//Disables Input Actions
     void OnDisable(){
        forwardMovement.Disable();
        sideMovement.Disable();
        meleeMovement.Disable();
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

 /*
SCRAPPED CODE FOR MOVEMENT (POSSIBLY TO BE USED LATER ON/REVISTED)
-This code is intended for the FixedUpdate() method. 

*/



    /*
    void FixedUpdate()
    {
        //Gets value of key presses for side and forward movement
        float sideInput = sideMovement.ReadValue<float>();
        float forwardInput = forwardMovement.ReadValue<float>();

//Allows the player to move forward and backward with W and S, regardless of rotation
        if(forwardInput != 0f)
            rb.velocity = transform.forward * moveSpeed * forwardInput * Time.fixedDeltaTime;
        
//Rotates the player left and right
        if(sideInput != 0f)
             transform.Rotate(new Vector3(0, sideInput * 1, 0) * Time.fixedDeltaTime * rotationSpeed, Space.World);
    }

    
        

//Rotates the player with the mouse (currently buggy)
    void RotateWithMouse(){
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotate = new Vector3(x, y * sensitivity, 0);
        Quaternion playerRotation = Quaternion.LookRotation(rotate, Vector3.up);
        
        transform.eulerAngles = transform.eulerAngles - rotate;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, rotationSpeed * Time.deltaTime);
        
        
    }
*/


   

//Converts this to a vector based on the movement
        //Vector3 directionToMove = new Vector3(sideInput, 0, forwardInput);
        //directionToMove.Normalize();


        //if(directionToMove == Vector3.zero)
            //return;

//Possible Rotation Script
/*
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToMove.x, 0, 0));
        targetRotation = Quaternion.RotateTowards(transform.rotation,
        targetRotation,
        rotationSpeed * Time.deltaTime);

        rb.MoveRotation(targetRotation);
*/
        //rb.MovePosition(rb.position + directionToMove * moveSpeed * Time.deltaTime);

        


        

//Multiplies movement vector by deltaTime and the speed to move by
        //movementForce = new Vector3(sideInput * moveSpeed * Time.deltaTime, 0, forwardInput * moveSpeed * Time.deltaTime);
        //transform.Translate(movementForce, Space.World);

       

/*
        if(movementForce != Vector3.zero && directionToMove != Vector3.zero){
            Quaternion playerRotation = Quaternion.LookRotation(new Vector3(directionToMove.x,0f,0f), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, rotationSpeed * Time.deltaTime);
        }
*/


        //RotateWithMouse();

        /*
   Vector3 mousePosition = Input.mousePosition;
   Vector3 direction = new Vector3(mousePosition.x-transform.position.x, mousePosition.y-transform.position.y, 0);
   transform.right = direction; 
   //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
   
*/

 //movementForce = new Vector3(sideInput * moveSpeed * Time.deltaTime, 0, forwardInput * moveSpeed * Time.deltaTime);
    //transform.Translate(movementForce, Space.World);

}




