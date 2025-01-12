using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Code by Jessie Archer
public class Player : MonoBehaviour
{

/*
This code largely pertains to a tutorial I found on rotating the player with
the mouse.
*/
    private float x;//X of Mouse
    private float y;//Y of Mouse
    [SerializeField]
    private float sensitivity;//Camera sensitivity 

    private Vector3 rotate;//Stores x + y vector to rotate by

    public CharacterController controller;

    public Transform cam;

/*
Player Fields 
*/
    [SerializeField]
    private float moveSpeed, rotationSpeed;//floats representing speed to move by

    [SerializeField]
    private InputAction forwardMovement, sideMovement, meleeMovement;//InputActions to be mapped to player actions

    private Vector3 movementForce;//A force to move by

    [SerializeField]
    private GameObject prefab;//The player's prefab (to be used later for Photon)

    [SerializeField]
    private Color color;//The player color 

    [SerializeField]
    private Rigidbody rb;


    // variables below are used for turning the player
    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;


    // Start is called before the first frame update
    void Start()
    {
        //Locks cursor for rotating with mouse (commented out for now)
        //Cursor.lockState = CursorLockMode.Locked;

        //Adds player's character controller to itself
        controller = gameObject.GetComponent<CharacterController>();

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

    // Update is called once per frame
    void FixedUpdate()
    {
        //Gets value of key presses for side and forward movement
        float sideInput = sideMovement.ReadValue<float>();
        float forwardInput = forwardMovement.ReadValue<float>();
        Vector3 direction = new Vector3(sideInput, 0f, forwardInput).normalized;

//Allows the player to move forward and backward with W and S, regardless of rotation
        if(direction.magnitude != 0f) {
            // rb.velocity = transform.forward * moveSpeed * forwardInput * Time.fixedDeltaTime;

            // makes player face the direction they're moving
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
            
        
//Rotates the player left and right
        // if(sideInput != 0f)
        //      rb.velocity = transform.right.normalized * moveSpeed * sideInput * Time.fixedDeltaTime;
    }
        

//Rotates the player with the mouse (currently buggy)
    // void RotateWithMouse(){
    //     y = Input.GetAxis("Mouse X");
    //     x = Input.GetAxis("Mouse Y");
    //     rotate = new Vector3(x, y * sensitivity, 0);
    //     Quaternion playerRotation = Quaternion.LookRotation(rotate, Vector3.up);
        
    //     transform.eulerAngles = transform.eulerAngles - rotate;
    //     transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, rotationSpeed * Time.deltaTime);
        
        
    // }



    /*
SCRAPPED CODE FOR MOVEMENT (POSSIBLY TO BE USED LATER ON/REVISTED)
-This code is intended for the FixedUpdate() method. 

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

}




