using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private float x;
    private float y;
    [SerializeField]
    private float sensitivity; 

    private Vector3 rotate;

    [SerializeField]
    private float moveSpeed, rotationSpeed, jumpHeight;

    [SerializeField]
    private InputAction forwardMovement, sideMovement;

    private Vector3 movementForce; 

    [SerializeField]
    private GameObject prefab;

    

    
    // Start is called before the first frame update
    void Start()
    {
        //Locks cursor for rotating with mouse
        //Cursor.lockState = CursorLockMode.Locked;

    }

//Enables Input Actions
     void OnEnable(){
        forwardMovement.Enable();
        sideMovement.Enable();
    }

//Disables Input Actions
     void OnDisable(){
        forwardMovement.Disable();
        sideMovement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Gets value of key presses for side and forward movement
        float sideInput = sideMovement.ReadValue<float>();
        float forwardInput = forwardMovement.ReadValue<float>();

//Converts this to a vector based on the movement
        Vector3 directionToMove = new Vector3(sideInput, 0, forwardInput);

//Multiplies movement vector by deltaTime and the speed to move by
        movementForce = new Vector3(sideInput * moveSpeed * Time.deltaTime, 0, forwardInput * moveSpeed * Time.deltaTime);
        transform.Translate(movementForce, Space.World);

        //RotateWithMouse();

    }

//Rotates the player with the mouse
    void RotateWithMouse(){
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotate = new Vector3(x, y * sensitivity, 0);
        Quaternion playerRotation = Quaternion.LookRotation(rotate, Vector3.up);
        
        transform.eulerAngles = transform.eulerAngles - rotate;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, rotationSpeed * Time.deltaTime);
        
        
    }
}
